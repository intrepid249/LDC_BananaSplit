using System.Threading;
using System.Threading.Tasks;

namespace LDC_BananaSplit
{
    internal class Program
    {
        // Create a way to safely cancel threads
        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private static Task Main(string[] args)
            => new Core(_cts).InitialiseAsync();
    }
}
