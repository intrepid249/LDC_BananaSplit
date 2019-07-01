using Discord.Commands;
using Discord.WebSocket;
using LDC_BananaSplit.System.Context;
using System;
using System.Threading.Tasks;
using System.IO;
using Discord;
using LDC_BananaSplit.System.Attributes;

namespace LDC_BananaSplit.Modules.admin
{
    [Summary("Utilities that help with managing the server")]
    public class Utils : ModuleBase<PrefixCommandContext>
    {
        [AdminPrefix]
        [Command("SetNominationChannel")]
        [Summary("Set the channel member recruit messages are sent to")]
        public async Task SetNominationChannel([Summary("#channel")]SocketGuildChannel channel)
        {
            Global.MemberNominationChannel = channel.Id;

            // Save the channel to a file to preserve data
            //Core.ConfigurationData.WriteString("ChannelData", "NominationChannel", channel.Id.ToString());
            Core.iniConfig.WriteValue("ChannelData", "NominationChannel", channel.Id.ToString());
            Core.iniConfig.Save();

            if (Core.iniConfig.GetValue("ChannelData", "NominationChannel") != null || !Core.iniConfig.GetValue("ChannelData", "NominationChannel").Equals(""))
            {
                Embed eb = new EmbedBuilder()
                    .WithTitle("Initialisation")
                    .WithDescription($"Nomination channel initialised to {Context.Guild.GetTextChannel(ulong.Parse(Core.iniConfig.GetValue("ChannelData", "NominationChannel"))).Mention}")
                    .Build();
                var response = await Context.Channel.SendMessageAsync(null, embed: eb);
                await Task.Delay(3000);
                await Context.Channel.DeleteMessageAsync(response);
            }
        }
    }
}
