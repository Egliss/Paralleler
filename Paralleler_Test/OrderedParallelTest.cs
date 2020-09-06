using System;
using System.Threading;
using System.Threading.Tasks;
using Egliss.Paralleler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Paralleler_Test
{
    [TestClass]
    public class OrderedParallelTest
    {
        [TestMethod]
        public async Task ResultTest()
        {
            var t0 = 0;
            var t1 = 0;
            var t2 = 0;
            var t3 = 0;

            await OrderedParallel.ForAsync(0, 0, (int index) => t0 += index);
            await OrderedParallel.ForAsync(0, -1, (int index) => t1 += index);
            await OrderedParallel.ForAsync(-1, 1, (int index) => t2 += index);
            await OrderedParallel.ForAsync(-1, -1, (int index) => t3 += index);

            Assert.AreEqual(0, t0);
            Assert.AreEqual(0, t1);
            Assert.AreEqual(-1, t2);
            Assert.AreEqual(0, t3);
        }
        [TestMethod]
        public async Task CancelTest()
        {
            var t0 = 0;
            var t0Token = new CancellationTokenSource();
            var orderTask = Task.Run(() => OrderedParallel.ForAsync(0, 1, async (index, token) =>
            {
                t0 += 1;
                if (token.IsCancellationRequested)
                    return;
                t0 += 1;
                await Task.Delay(200);
                if (token.IsCancellationRequested)
                    return;
                t0 += 1;
            }, t0Token.Token, 1));
            await Task.Delay(100);
            t0Token.Cancel();
            await orderTask;

            Assert.AreEqual(2, t0);
        }
        [TestMethod]
        public async Task OverThreadCountTest()
        {
            var t0 = 0;
            await OrderedParallel.ForAsync(0, 10, (int index) => t0 += 1, 32);
            Assert.AreEqual(10, t0);
        }
    }
}
