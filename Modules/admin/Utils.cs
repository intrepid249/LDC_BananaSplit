using Discord.Commands;
using Discord.WebSocket;
using LDC_BananaSplit.System.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LDC_BananaSplit.Modules.admin
{
    public class Utils : ModuleBase<PrefixCommandContext>
    {
        [Command("NominationChannel")]
        public async Task SetNominationChannel(SocketGuildChannel channel)
        {
            Global.MemberNominationChannel = channel.Id;
        }
    }
}
