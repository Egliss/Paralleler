using System;
using System.Threading;
using System.Threading.Tasks;

namespace Paralleler
{
    internal class ParallelForContext
    {
        private int _activeIndex = 0;
        private readonly int _endIndex = 0;
        private readonly int _runnerCount = 0;
        public ParallelForContext(int begin, int end, int threadCount)
        {
            this._activeIndex = begin;
            this._endIndex = end;
            if(threadCount == -1)
                threadCount = Environment.ProcessorCount / 2;
            this._runnerCount = Math.Max(1, Math.Min(threadCount, end - begin));
        }
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int> action, int threadCount)
        {
            var context = new ParallelForContext(beginIndex, endIndex, threadCount);
            var tasks = new Task[context._runnerCount];
            for(var index = 0; index < context._runnerCount; index++)
            {
                tasks[index] = Task.Run(() => context.RunNext(action));
            }
            await Task.WhenAll(tasks);
        }
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int, CancellationToken> action, CancellationToken token, int threadCount = -1)
        {
            var context = new ParallelForContext(beginIndex, endIndex, threadCount);
            var tasks = new Task[context._runnerCount];
            for(var index = 0; index < context._runnerCount; index++)
            {
                tasks[index] = Task.Run(() => context.RunNext(action, token));
            }
            await Task.WhenAll(tasks);
        }
        private void RunNext(Action<int> action)
        {
            var next = Interlocked.Increment(ref this._activeIndex) - 1;
            if(next >= this._endIndex)
                return;

            action(next);

            this.RunNext(action);
        }
        private void RunNext(Action<int, CancellationToken> action, CancellationToken token)
        {
            var next = Interlocked.Increment(ref this._activeIndex) - 1;
            if(next >= this._endIndex)
                return;
            if(token.IsCancellationRequested)
                return;

            action(next, token);

            this.RunNext(action, token);
        }
    }
}
