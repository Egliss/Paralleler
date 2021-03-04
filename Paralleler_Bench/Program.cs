using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Egliss.Paralleler;
using System.Threading.Tasks;

namespace Paralleler_Bench
{
    public class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<Program>();
        }
        [Benchmark(Description = "OrderedParallel.ForAsync(-1)")]
        public void ParallelerForAsyncM1()
        {
            OrderedParallel
                .ForAsync(0, 1000, (int i) => { }, -1)
                .Wait();
        }
        [Benchmark(Description = "Parallel.For(-1)")]
        public void ParallelForM1()
        {
            var opt = new ParallelOptions()
            {
                MaxDegreeOfParallelism = -1
            };
            Parallel.For(0, 1000, opt, (int o) => { });
        }
        [Benchmark(Description = "OrderedParallel.ForAsync(1)")]
        public void ParallelerForAsync1()
        {
            OrderedParallel
                .ForAsync(0, 1000, (int i) => { }, 1)
                .Wait();
        }
        [Benchmark(Description = "Parallel.For(1)")]
        public void ParallelFor1()
        {
            var opt = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 1
            };
            Parallel.For(0, 1000, opt, (int o) => { });
        }
        [Benchmark(Description = "OrderedParallel.ForAsync(4)")]
        public void ParallelerForAsync4()
        {
            OrderedParallel
                .ForAsync(0, 1000, (int i) => { }, 4)
                .Wait();
        }
        [Benchmark(Description = "Parallel.For(4)")]
        public void ParallelFor4()
        {
            var opt = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4
            };
            Parallel.For(0, 1000, opt, (int o) => { });
        }
        [Benchmark(Description = "OrderedParallel.ForAsync(8)")]
        public void ParallelerForAsync8()
        {
            OrderedParallel
                .ForAsync(0, 1000, (int i) => { }, 8)
                .Wait();
        }
        [Benchmark(Description = "Parallel.For(8)")]
        public void ParallelFor8()
        {
            var opt = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 8
            };
            Parallel.For(0, 1000, opt, (int o) => { });
        }
    }
}
