using Discord.Commands;
using LDC_BananaSplit.System.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LDC_BananaSplit.System.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Module | AttributeTargets.Class)]
    class AdminPrefixAttribute : PreconditionAttribute
    {
        private string Prefix;

        public AdminPrefixAttribute()
        {
            Prefix = Global.AdminPrefix;
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context is PrefixCommandContext && ((PrefixCommandContext)context).Prefix.Equals(Prefix))
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }

            return Task.FromResult(PreconditionResult.FromError("Invalid command prefix"));
        }
    }
}
