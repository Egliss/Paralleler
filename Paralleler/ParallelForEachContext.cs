using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Egliss
{
    public class ParallelForEachContext<T>
    {
        private IEnumerator<T> _activeIterator = null;
        private object _mutex = new object();
        private readonly int _runnerCount = 0;
        private readonly int _elementCount = 0;
        
        public ParallelForEachContext(IEnumerable<T> container, int threadCount)
        {
            this._activeIterator = container.GetEnumerator();
            this._elementCount = container.Count();
            if (threadCount == -1)
                threadCount = Environment.ProcessorCount / 2;
            this._runnerCount = Math.Max(1, Math.Min(threadCount, this._elementCount));
        }
        public static async Task ForEachAsync(IEnumerable<T> container, Action<T> action, int threadCount)
        {
            var context = new ParallelForEachContext<T>(container, threadCount);
            var tasks = new Task[context._runnerCount];
            for (var index = 0; index < context._runnerCount; index++)
            {
                tasks[index] = Task.Run(() => context.RunNext(action));
            }
            await Task.WhenAll(tasks);
        }
        public static async Task ForEachAsync(IEnumerable<T> container, Action<T, CancellationToken> action, CancellationToken token, int threadCount = -1)
        {
            var context = new ParallelForEachContext<T>(container, threadCount);
            var tasks = new Task[context._runnerCount];
            for (var index = 0; index < context._runnerCount; index++)
            {
                tasks[index] = Task.Run(() => context.RunNext(action, token));
            }
            await Task.WhenAll(tasks);
        }
        private void RunNext(Action<T> action)
        {
            IEnumerator<T> iterator;
            lock (this._mutex)
            {
                this._activeIterator.MoveNext();
                iterator = this._activeIterator;
            }
            var element = iterator.Current;
            if (element == null)
                return;

            action(element);

            this.RunNext(action);
        }
        private void RunNext(Action<T, CancellationToken> action, CancellationToken token)
        {
            IEnumerator<T> iterator;
            lock (this._mutex)
            {
                this._activeIterator.MoveNext();
                iterator = this._activeIterator;
            }
            var element = iterator.Current;
            if (element == null)
                return;
            if (token.IsCancellationRequested)
                return;

            action(element, token);

            this.RunNext(action, token);
        }
    }
}
