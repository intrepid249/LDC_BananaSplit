using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LDC_BananaSplit.Services;
using LDC_BananaSplit.System.Context;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using LDC_BananaSplit.System.Resources;

namespace LDC_BananaSplit
{
    public sealed class Core
    {
        private readonly IServiceProvider Services;
        private readonly CancellationTokenSource Cts;
        private readonly SemaphoreSlim ColorLock = new SemaphoreSlim(1, 1);

        public static IConfiguration Config;
#if DEBUG
        internal static Ini iniConfig = new Ini(Directory.GetCurrentDirectory() + "../../../config/configuration.ini");
#else
        internal static Ini iniConfig = new Ini(Directory.GetCurrentDirectory() + "/config/configuration.ini");
#endif

        private readonly CommandService Commands = new CommandService(new CommandServiceConfig
        {
            LogLevel = LogSeverity.Debug,
            DefaultRunMode = RunMode.Async
        });
        private readonly DiscordSocketClient Client = new DiscordSocketClient(new DiscordSocketConfig
        {
            MessageCacheSize = 100,
            LogLevel = LogSeverity.Debug
        });

        // Constructor
        public Core(CancellationTokenSource _cts)
        {
            Cts = _cts;

            Config = BuildConfig();
            iniConfig.Load();

            Services = ConfigureServices();
        }

        public async Task InitialiseAsync()
        {
            Client.MessageReceived += HandleMessageReceived;
            Client.Ready += async () =>
            {
                await Client.SetGameAsync("the ice melt", type: ActivityType.Listening);
            };
            Client.Log += LogAsync;
            Commands.Log += LogAsync;

            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);

            if (Config["token"].Equals(""))
            {
                // If no token, set the token
                Console.WriteLine("Please enter bot token: ");
                String token = Console.ReadLine();
                Config["token"] = token;
            }

            await Client.LoginAsync(TokenType.Bot, Config["token"]);
            await Client.StartAsync();

            await Task.Delay(-1, Cts.Token)
                .ContinueWith(async task =>
                {
                    await Client.StopAsync();
                    await Client.LogoutAsync();

                    Client.Dispose();
                });
        }

        private async Task HandleMessageReceived(SocketMessage _rawMsg)
        {
            // Ignore system messages and messages from bots
            if (!(_rawMsg is SocketUserMessage msg) || msg.Author.IsBot) return;

            ICommandContext context;

            int argPos = 0;
            if (msg.HasStringPrefix(Global.AdminPrefix, ref argPos))
            {
                context = new PrefixCommandContext(Global.AdminPrefix, Client, msg);
            }
            else if (msg.HasStringPrefix(Global.UserPrefix, ref argPos))
            {
                context = new PrefixCommandContext(Global.UserPrefix, Client, msg);
            }
            else return;

            IResult result = await Commands.ExecuteAsync(context, argPos, Services, MultiMatchHandling.Best);
        }

        private async Task LogAsync(LogMessage message)
        {
            if (message.Severity > LogSeverity.Verbose)
                return;
            try
            {
                await ColorLock.WaitAsync();
                ConsoleColor color = ConsoleColor.White;
                switch (message.Severity)
                {
                    case LogSeverity.Debug:
                        color = ConsoleColor.Gray;
                        break;
                    case LogSeverity.Verbose:
                        color = ConsoleColor.White;
                        break;
                    case LogSeverity.Info:
                        color = ConsoleColor.Green;
                        break;
                    case LogSeverity.Warning:
                        color = ConsoleColor.DarkYellow;
                        break;
                    case LogSeverity.Error:
                    case LogSeverity.Critical:
                        color = ConsoleColor.Red;
                        break;
                }
                Console.ForegroundColor = color;

                Console.WriteLine($"[{message.Severity.ToString().PadRight(7)}] {message.Source.PadRight(10)}@{DateTimeOffset.UtcNow.ToString("HH:mm:ss dd/mm")}\t" +
                    $"{message.Message}{(message.Exception != null ? Environment.NewLine : "")}{message.Exception?.Message ?? ""}" +
                    $"{(message.Exception != null ? Environment.NewLine : "")}{message.Exception?.StackTrace ?? ""}");
            }
            finally
            {
                ColorLock.Release();
            }
        }

        private IConfiguration BuildConfig()
            // Build and read a new configuration file from the file system
            => new ConfigurationBuilder()
#if DEBUG
                    .SetBasePath(Directory.GetCurrentDirectory() + "../../../config")
#else
                    .SetBasePath(Directory.GetCurrentDirectory() + "/config")
#endif
                    .AddJsonFile("config.json")
                    .Build();

        private IServiceProvider ConfigureServices()
            => new ServiceCollection()
            .AddSingleton(Client)
            .AddSingleton(new AddReactionService(Client))
            .BuildServiceProvider();
    }
}
