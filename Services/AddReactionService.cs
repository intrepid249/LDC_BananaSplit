using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LDC_BananaSplit.Services
{
    class AddReactionService
    {
        private readonly DiscordSocketClient Client;

        public AddReactionService(DiscordSocketClient _client)
        {
            Client = _client;

            Client.ReactionAdded += HandleReactionAdded;
        }

        private async Task HandleReactionAdded(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (((SocketUser)reaction.User).IsBot) return;

            //await (await channel.SendMessageAsync($"{reaction.User} added a reaction")).AddReactionAsync(reaction.Emote);

            // Self assigned roles
            if (msg.Id == Global.AutoRoleMessageID)
            {

            }
        }
    }
}
