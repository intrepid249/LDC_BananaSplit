using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace LDC_BananaSplit.System.Context
{
    public class PrefixCommandContext : SocketCommandContext
    {
        public string Prefix { get; }

        public PrefixCommandContext(string _prefix, DiscordSocketClient _client, SocketUserMessage _msg) : base(_client, _msg)
        {
            Prefix = _prefix;
        }
    }
}
