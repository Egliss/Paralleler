using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Egliss.Paralleler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Paralleler_Test
{
    [TestClass]
    public class OrderedParallelForEachTest
    {
        [TestMethod]
        public async Task ResultTest()
        {
            var t0 = new List<int>()
            {
                0,1,-2,3,4,5,-6,7,8,9
            };
            var t1 = new List<int>();
            var t0Result = 0;
            var t1Result = 0;
            await OrderedParallel.ForEachAsync(t0, (value) => t0Result += value);
            await OrderedParallel.ForEachAsync(t1, (value) => t1Result = 100);

            Assert.AreEqual(t0.Sum(), t0Result);
            Assert.AreEqual(0, t1Result);
        }
        [TestMethod]
        public async Task CancelTest()
        {
            var t0Result = 0;
            var t0 = new List<int>()
            { 0 };
            var t0Token = new CancellationTokenSource();
            var orderTask = Task.Run(() => OrderedParallel.ForEachAsync(t0, async (value, token) =>
            {
                t0Result += 1;
                if (token.IsCancellationRequested)
                    return;
                t0Result += 1;
                await Task.Delay(200);
                if (token.IsCancellationRequested)
                    return;
                t0Result += 1;
            }, t0Token.Token, 1));
            await Task.Delay(100);
            t0Token.Cancel();
            await orderTask;

            Assert.AreEqual(2, t0Result);
        }
        [TestMethod]
        public async Task OverThreadCountTest()
        {
            var t0 = new List<int>()
            {
                0,1,2,3,4,5,6,7,8,9
            };
            var t0Result = 0;

            await OrderedParallel.ForEachAsync(t0, (int index) =>
            {
                t0Result += 1;
                Console.WriteLine(index);
            }
            , 32);
            Assert.AreEqual(t0.Count, t0Result);
        }
    }
}
