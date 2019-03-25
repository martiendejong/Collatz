using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Collatz;
using System.Threading;

namespace UnitTestProject1
{
    [TestClass]
    public class CollatzTest
    {
        [TestMethod]
        public void Collatz2ShouldReturn2StepsForAnOddNumber()
        {
            var random = new Random();
            for(var i = 0; i < 1000; ++i)
            {
                ulong n = ((ulong)random.Next(10000000) * 2) - 1;
                var collatz2 = CollatzCalculator.Collatz2(n);
                var test = CollatzCalculator.Collatz(CollatzCalculator.Collatz(n));

                Assert.AreEqual(collatz2, test);
            }
        }

        [TestMethod]
        public void Collatz2QuickShouldReturnTheSameAsCollatz2()
        {
            var random = new Random();
            for (var i = 0; i < 1000; ++i)
            {
                ulong n = (ulong)random.Next(10000000);
                var collatz2Quick = CollatzCalculator.Collatz2Quick(n);
                var collatz2 = CollatzCalculator.Collatz2(n);

                Assert.AreEqual(collatz2, collatz2Quick);
            }
        }

        [TestMethod]
        public void Collatz2QuickShouldRunFasterThenCollatz2()
        {
            var start = DateTime.Now;
            for (ulong i = 0; i < 10000; ++i)
            {
                CollatzCalculator.Collatz2(i);
            }
            var end = DateTime.Now;
            var durationCollatz2 = end - start;

            start = DateTime.Now;
            for (ulong i = 0; i < 10000; ++i)
            {
                CollatzCalculator.Collatz2Quick(i);
            }
            end = DateTime.Now;
            var durationCollatz2Quick = end - start;

            Assert.IsTrue(durationCollatz2Quick < durationCollatz2);
        }

        [TestMethod]
        public void CollatzTriangleResultShouldAppearInSequenceOfCollatz2()
        {
            var random = new Random();
            for (var i = 0; i < 1000; ++i)
            {
                //ulong n = (ulong)random.Next(100);
                ulong n = (ulong)i;
                var collatzTriangle = CollatzCalculator.CollatzPrimeTriangleResult(n);
                var collatz = CollatzCalculator.Collatz2Quick(n);
                while(collatz != collatzTriangle && collatz > 1)
                {
                    collatz = CollatzCalculator.Collatz2Quick(collatz);
                }

                Assert.AreEqual(collatz, collatzTriangle);
            }
        }
    }
}
