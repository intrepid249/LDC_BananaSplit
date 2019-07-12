using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace LDC_BananaSplit.Services
{
    class MemberJoinService
    {
        private readonly DiscordSocketClient Client;

        public MemberJoinService(DiscordSocketClient _client)
        {
            Client = _client;
            Client.UserJoined += HandleUserJoin;
        }

        private async Task HandleUserJoin(SocketGuildUser user)
        {
            // If user isn't a bot, give them the user role and display a nice welcome message?
            if (!user.IsBot)
                await user.AddRoleAsync(user.Guild.GetRole(567276137093398528));

            // If the user is a bot, give them the bot role
            if (user.IsBot)
            {
                await user.AddRoleAsync(user.Guild.GetRole(537509783767482381));
            }
        }
    }
}
