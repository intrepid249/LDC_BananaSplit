using Discord.Commands;
using Discord.WebSocket;
using LDC_BananaSplit.System.Context;
using System;
using IniParser;
using IniParser.Model;
using System.Threading.Tasks;
using System.IO;
using Discord;

namespace LDC_BananaSplit.Modules.admin
{
    public class Utils : ModuleBase<PrefixCommandContext>
    {
        [Command("SetNominationChannel")]
        public async Task SetNominationChannel(SocketGuildChannel channel)
        {
            Global.MemberNominationChannel = channel.Id;

            try
            {
                // Save the channel to a file to preserve data
                Global.ConfigurationData["ChannelData"]["NominationChannel"] = channel.Id.ToString();
#if DEBUG
                Global.IniParser.WriteFile(Directory.GetCurrentDirectory() + "../../../config/" + "configuration.ini", Global.ConfigurationData);
#else
                Global.IniParser.WriteFile("configuration.ini", Global.ConfigurationData);
#endif
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            if (Global.ConfigurationData["ChannelData"]["NominationChannel"] != null)
            {
                Embed eb = new EmbedBuilder()
                    .WithTitle("Initialisation")
                    .WithDescription($"Nomination channel initialised to {Context.Guild.GetTextChannel(ulong.Parse(Global.ConfigurationData["ChannelData"]["NominationChannel"])).Mention}")
                    .Build();
                var response = await Context.Channel.SendMessageAsync(null, embed: eb);
                await Task.Delay(3000);
                await Context.Channel.DeleteMessageAsync(response);
            }
        }
    }
}
