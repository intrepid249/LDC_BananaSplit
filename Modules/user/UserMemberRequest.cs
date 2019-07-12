using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LDC_BananaSplit.System.Attributes;
using LDC_BananaSplit.System.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LDC_BananaSplit.Modules.user
{
    [Summary("A list of commands for requesting and approving membership")]
    public class UserMemberRequest : ModuleBase<PrefixCommandContext>
    {
        [UserPrefix]
        [Command("recruit")]
        [Summary("Request membership to the registered nomination channel")]
        public async Task MemberRequest()
        {
            if (Core.iniConfig.GetValue("ChannelData", "NominationChannel") == null || Core.iniConfig.GetValue("ChannelData", "NominationChannel").Equals(""))
            {
                Embed eb = new EmbedBuilder()
                    .WithTitle("❌ Error")
                    .WithDescription("The nomination channel has not been initialised. Please contact a server Admin to resolve this issue")
                    .WithColor(255, 0, 0)
                    .Build();

                var error = await Context.Channel.SendMessageAsync(null, embed: eb);
                await Task.Delay(3000);
                await Context.Channel.DeleteMessageAsync(error);

                return;
            }

            // Increment the unique ID code for the nomination and assign it to the user if they haven't already nominated
            Global.MemberNominationID++;
            SocketGuildUser user = (SocketGuildUser)Context.Message.Author;
            if (!Global.recruitList.ContainsValue(user))
            {
                Global.recruitList.Add(Global.MemberNominationID, user);
            }
            else
            {
                Embed eb = new EmbedBuilder()
                    .WithTitle("❌ Error")
                    .WithDescription("You have already nominated yourself for server membership")
                    .WithColor(255, 0, 0)
                    .Build();

                var error = await Context.Channel.SendMessageAsync(null, embed: eb);
                await Task.Delay(3000);
                await Context.Channel.DeleteMessageAsync(error);

                return;
            }


            Embed recruitEb = new EmbedBuilder()
                .WithTitle("Awaiting Approval")
                .WithDescription("Your request has been logged, and the leadership team has been notified")
                .WithColor(174, 103, 229)
                .Build();

            await Context.Channel.SendMessageAsync(null, embed: recruitEb);

            Embed nominationEb = new EmbedBuilder()
                .WithTitle("Member Recruitment Request")
                .WithDescription($"{user.Mention} has requested to join the ranks of members. What say thee?")
                .WithFooter($"Nomination ID: {Global.MemberNominationID}")
                .WithColor(174, 103, 229)
                .Build();

            var nominationChannel = Context.Guild.GetTextChannel(ulong.Parse(Core.iniConfig.GetValue("ChannelData", "NominationChannel")));

            var msg = await nominationChannel.SendMessageAsync(null, embed: nominationEb);
            await msg.AddReactionAsync(new Emoji("👍"));
            await msg.AddReactionAsync(new Emoji("👎"));
            await msg.AddReactionAsync(new Emoji("🤔"));
        }

        [AdminPrefix]
        [Command("Approve"), RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Not yet implemented")]
        public async Task ApproveMembership([Summary("@user")]SocketGuildUser user)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            await Context.Message.DeleteAsync();

            IRole playerRole = user.Guild.GetRole(567276137093398528);
            IRole memberRole = user.Guild.GetRole(537496014647459842);

            bool hasPlayerRole = false;
            bool hasMemberRole = false;

            foreach (var role in user.Roles)
            {
                if (role == playerRole)
                    hasPlayerRole = true;

                if (role == memberRole)
                    hasMemberRole = true;
            }

            // Add the member role if the user has the player role, but NOT already a member
            if (hasPlayerRole && !hasMemberRole)
            {
                await user.RemoveRoleAsync(playerRole);
                await user.AddRoleAsync(memberRole);
            }

            Embed approveEb = new EmbedBuilder()
                .WithTitle("Membership Approved")
                .WithDescription($"Congratulations {user.Mention} on receiving your membership to LDC")
                .WithColor(174, 103, 229)
                .WithFooter($"Please enjoy the view")
                .Build();

            // Send a nice congratulatory message to the user
            var approve = await user.Guild.GetTextChannel(591904276788281352).SendMessageAsync(null, embed: approveEb);
            await approve.AddReactionAsync(new Emoji("🎉"));
        }
    }
}
