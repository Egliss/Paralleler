using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Paralleler
{
    public static class OrderedParallel
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int> action)
        {
            if(!ThrowIfInvalidForArgument(beginIndex, endIndex, action))
                return;

            await ParallelForContext.ForAsync(beginIndex, endIndex, action, -1);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int> action, int threadCount)
        {
            if(!ThrowIfInvalidForArgument(beginIndex, endIndex, action))
                return;

            await ParallelForContext.ForAsync(beginIndex, endIndex, action, threadCount);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int, CancellationToken> action, CancellationToken token, int threadCount = -1)
        {
            if(!ThrowIfInvalidForArgument(beginIndex, endIndex, action))
                return;

            await ParallelForContext.ForAsync(beginIndex, endIndex, action, token, threadCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task ForEachAsync<T>(IEnumerable<T> container, Action<T> action)
        {
            ThrowIfInvalidForEachArgument(container, action);
            await ParallelForEachContext<T>.ForEachAsync(container, action, -1);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task ForEachAsync<T>(IEnumerable<T> container, Action<T> action, int threadCount)
        {
            ThrowIfInvalidForEachArgument(container, action);
            await ParallelForEachContext<T>.ForEachAsync(container, action, threadCount);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task ForEachAsync<T>(IEnumerable<T> container, Action<T, CancellationToken> action, CancellationToken token, int threadCount = -1)
        {
            ThrowIfInvalidForEachArgument(container, action);
            await ParallelForEachContext<T>.ForEachAsync(container, action, token, threadCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ThrowIfInvalidForArgument<T>(int beginIndex, int endIndex, T action) where T : class
        {
            if(beginIndex >= endIndex)
                return false;
            if(action == null)
                throw new ArgumentNullException(nameof(action));
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowIfInvalidForEachArgument<T, U>(IEnumerable<T> container, U action) where U : class
        {
            if(container == null)
                throw new ArgumentNullException(nameof(container));
            if(action == null)
                throw new ArgumentNullException(nameof(action));
        }
    }
}
