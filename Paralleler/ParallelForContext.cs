using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Egliss
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
            if (threadCount == -1)
                threadCount = Environment.ProcessorCount / 2;
            this._runnerCount = Math.Max(1, Math.Min(threadCount, end - begin));
        }
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int> action, int threadCount)
        {
            var context = new ParallelForContext(beginIndex, endIndex, threadCount);
            var tasks = new Task[context._runnerCount];
            for (var index = 0; index < context._runnerCount; index++)
            {
                var next = Interlocked.Increment(ref context._activeIndex) - 1;
                if (next >= context._endIndex)
                    return;
                tasks[index] = Task.Run(() => context.RunNext(next, action));
            }
            await Task.WhenAll(tasks);
        }
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int, CancellationToken> action, CancellationToken token, int threadCount = -1)
        {
            var context = new ParallelForContext(beginIndex, endIndex, threadCount);
            var tasks = new Task[context._runnerCount];
            for (var index = 0; index < context._runnerCount; index++)
            {
                var next = Interlocked.Increment(ref context._activeIndex) - 1;
                if (next >= context._endIndex)
                    return;
                tasks[index] = Task.Run(() => context.RunNext(next, action, token));
            }
            await Task.WhenAll(tasks);
        }
        private void RunNext(int index, Action<int> action)
        {
            action(index);
            var next = Interlocked.Increment(ref this._activeIndex) - 1;
            if (next >= this._endIndex)
                return;

            this.RunNext(next, action);
        }
        private void RunNext(int index, Action<int, CancellationToken> action, CancellationToken token)
        {
            action(index, token);
            var next = Interlocked.Increment(ref this._activeIndex) - 1;
            if (next >= this._endIndex)
                return;
            if (token.IsCancellationRequested)
                return;

            this.RunNext(next, action, token);
        }
    }
}
