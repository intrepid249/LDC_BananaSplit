using Discord.Commands;
using LDC_BananaSplit.System.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LDC_BananaSplit.System.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Module | AttributeTargets.Method)]
    class UserPrefixAttribute : PreconditionAttribute
    {
        private string _prefix;

        public UserPrefixAttribute()
        {
            _prefix = Global.UserPrefix;
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {

            if (context is PrefixCommandContext && ((PrefixCommandContext)context).Prefix == _prefix)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }

            return Task.FromResult(PreconditionResult.FromError("Invalid command prefix"));
        }
    }
}
