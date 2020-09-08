using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Egliss.Paralleler
{
    public static class OrderedParallel
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int> action)
        {
            if (ThrowIfInvalidForArgument(beginIndex, endIndex, action))
                return;

            await ParallelForContext.ForAsync(beginIndex, endIndex, action, -1);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int> action, int threadCount)
        {
            if (ThrowIfInvalidForArgument(beginIndex, endIndex, action))
                return;

            await ParallelForContext.ForAsync(beginIndex, endIndex, action, threadCount);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int, CancellationToken> action, CancellationToken token, int threadCount = -1)
        {
            if (ThrowIfInvalidForArgument(beginIndex, endIndex, action))
                return;

            await ParallelForContext.ForAsync(beginIndex, endIndex, action, token, threadCount);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ThrowIfInvalidForArgument<T>(int beginIndex, int endIndex, T action) where T : class
        {
            if (beginIndex >= endIndex)
                return false;
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            return true;
        }
    }
}
