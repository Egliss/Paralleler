using System;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Egliss.Paralleler;

namespace Paralleler_Bench
{
    public class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<Program>();
        }

        [Benchmark(Description = "Egliss.Paralleler.OrderedParallel.ForAsync")]
        public void ParallelerForAsync()
        {
            var token = new CancellationTokenSource();
            var opt = new ParallelOptions()
            {
                CancellationToken = token.Token
            };
            OrderedParallel
                .ForAsync(0, 100, opt, (int o) => { })
                .Wait();
        }

        [Benchmark(Description = "System.Threading.Tasks.Parallel.For")]
        public void ParallelFor()
        {
            var token = new CancellationTokenSource();
            var opt = new ParallelOptions()
            {
                CancellationToken = token.Token
            };
            Parallel.For(0, 100, opt, (int o) => { });
        }
    }
}
