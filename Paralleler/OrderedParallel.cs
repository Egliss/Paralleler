using System;
using System.Threading;
using System.Threading.Tasks;

namespace Egliss.Paralleler
{
    public class OrderedParallel
    {
        private int _activeIndex = 0;
        private readonly int _endIndex = 0;

        public OrderedParallel(int endIndex)
        {
            this._endIndex = endIndex;
        }

        public static async Task ForAsync(int beginIndex, int endIndex, ParallelOptions options, Action<int> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (endIndex <= beginIndex)
                return;

            var context = new OrderedParallel(endIndex);
            var runnableCount = Math.Max(1, Math.Min(options.MaxDegreeOfParallelism, endIndex - beginIndex));
            var tasks = new Task[runnableCount];
            for (var index = 0; index < runnableCount; index++)
            {
                var next = Interlocked.Increment(ref context._activeIndex) - 1;
                tasks[index] = Task.Run(() => context.RunNext(next, action, options.CancellationToken));
            }
            await Task.WhenAll(tasks);
        }
        public static async Task ForAsync(int beginIndex, int endIndex, int threadCount, Action<int> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (endIndex <= beginIndex)
                return;

            var context = new OrderedParallel(endIndex);
            var runnableCount = Math.Max(1, Math.Min(threadCount, endIndex - beginIndex));
            var tasks = new Task[runnableCount];
            for (var index = 0; index < runnableCount; index++)
            {
                var next = Interlocked.Increment(ref context._activeIndex) - 1;
                tasks[index] = Task.Run(() => context.RunNext(next, action));
            }
            await Task.WhenAll(tasks);
        }
        private void RunNext(int index, Action<int> action, CancellationToken token)
        {
            action(index);
            var next = Interlocked.Increment(ref this._activeIndex) - 1;
            if (next >= this._endIndex)
                return;

            this.RunNext(next, action, token);
        }
        private void RunNext(int index, Action<int> action)
        {
            action(index);
            var next = Interlocked.Increment(ref this._activeIndex) - 1;
            if (next >= this._endIndex)
                return;

            this.RunNext(next, action);
        }
    }
}
