using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LDC_BananaSplit.System.Attributes;
using LDC_BananaSplit.System.Context;
using System;
using System.Threading.Tasks;

namespace LDC_BananaSplit.Modules.admin
{
    [AdminPrefix]
    public class MemberVote : ModuleBase<PrefixCommandContext>
    {
        [Command("vote")]
        public async Task Poll([Remainder]String pollTopic)
        {
            if (pollTopic == null || pollTopic.Equals(""))
            {
                var error = await Context.Channel.SendMessageAsync(":X: You must include the topic for the poll :X:");
                await Task.Delay(TimeSpan.FromSeconds(3));
                await Context.Channel.DeleteMessageAsync(error);
            }

            Embed embed = new EmbedBuilder()
                .WithTitle(pollTopic)
                .WithColor(174, 103, 229)
                .Build();

            var msg = await Context.Channel.SendMessageAsync(null, embed: embed);
            await msg.AddReactionAsync(new Emoji("👍"));
            await msg.AddReactionAsync(new Emoji("👎"));
            await msg.AddReactionAsync(new Emoji("🤔"));
        }
    }
}
