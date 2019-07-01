using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LDC_BananaSplit
{
    internal class Global
    {

        internal static Dictionary<ulong, SocketGuildUser> recruitList = new Dictionary<ulong, SocketGuildUser>();


        internal static String UserPrefix { get; } = Core.Config["prefix"];
        internal static String AdminPrefix { get; } = Core.Config["adminprefix"];

        internal static ulong AutoRoleMessageID { get; set; }

        internal static ulong MemberNominationChannel { get; set; }
        internal static ulong MemberNominationID { get; set; } = 0;
    }
}
