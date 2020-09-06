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
            if (beginIndex >= endIndex)
                return;
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            await ParallelForContext.ForAsync(beginIndex, endIndex, action, -1);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int> action, int threadCount)
        {
            if (beginIndex >= endIndex)
                return;
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            await ParallelForContext.ForAsync(beginIndex, endIndex, action, threadCount);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task ForAsync(int beginIndex, int endIndex, Action<int, CancellationToken> action, CancellationToken token, int threadCount = -1)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (endIndex <= beginIndex)
                return;

            await ParallelForContext.ForAsync(beginIndex, endIndex, action, token, threadCount);
        }
    }
}
