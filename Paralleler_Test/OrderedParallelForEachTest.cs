using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Egliss.Paralleler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Paralleler_Test
{
    [TestClass]
    class OrderedParallelForEachTest
    {
        [TestMethod]
        public async Task ResultTest()
        {
            var t0 = new List<int>()
            {
                0,1,2
            };
            var t1 = new List<int>()
            {};
            var t0Result = 0;
            var t1Result = 0;
            await OrderedParallel.ForEachAsync(t0, (value) => t0Result += value);
            await OrderedParallel.ForEachAsync(t1, (value) => t1Result = 100);

            Assert.AreEqual(3, t0);
            Assert.AreEqual(100, t1);
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
