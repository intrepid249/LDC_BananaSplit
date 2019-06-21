using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LDC_BananaSplit.System.Context;
using System;
using System.Threading.Tasks;

namespace LDC_BananaSplit.Modules.user
{
    public class UserMemberRequest : ModuleBase<PrefixCommandContext>
    {
        [Command("recruit")]
        public async Task MemberRequest()
        {
            SocketGuildUser user = (SocketGuildUser)Context.Message.Author;

            Embed embed = new EmbedBuilder()
                .WithTitle("Member Recruitment Request")
                .WithDescription($"{user.Mention} has requested to join the ranks of members. What say thee?")
                .WithColor(174, 103, 229)
                .Build();

            var msg = await Context.Channel.SendMessageAsync(null, embed: embed);
            await msg.AddReactionAsync(new Emoji("👍"));
            await msg.AddReactionAsync(new Emoji("👎"));
            await msg.AddReactionAsync(new Emoji("🤔"));
        }
    }
}
