using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collatz
{
    /// <summary>
    /// Calculates the collatz sequence for a number.
    /// </summary>
    public class CollatzCalculator
    {
        /// <summary>
        /// Calculates the next collatz number. 
        /// </summary>
        /// <param name="n"></param>
        /// <returns>If odd, n*3+1. If even, n/2.</returns>
        public static ulong Collatz(ulong n)
        {
            return n % 2 == 0 ? n / 2 : n * 3 + 1;
        }

        /// <summary>
        /// Calculates 2 steps n+((n+1)/2 if the input is an odd number. Otherwise calculates just the n/2.
        /// </summary>
        /// <param name="n"></param>
        /// <returns>n+((n+1)/2) if the input is an odd number. Otherwise n/2</returns>
        public static ulong Collatz2(ulong n)
        {
            return n % 2 == 0 ? n / 2 : n + ((n + 1) / 2);
        }

        /// <summary>
        /// Calculates 2 steps (1.5 * (n + 1)) - 1 if the input is an odd number. Otherwise calculates just the n/2.
        /// </summary>
        /// <param name="n"></param>
        /// <returns>(1.5 * (n + 1)) - 1 if the input is an odd number. Otherwise n/2</returns>
        public static ulong Collatz2Quick(ulong n)
        {
            return n % 2 == 0 ? n / 2 : (ulong)(1.5 * (n + 1)) - 1;
        }

        /// <summary>
        /// Calculates 4 steps of Collatz2, a Collatz Prime. Keeps track of if 2 times divided
        /// </summary>
        /// <param name="n"></param>
        /// <returns>(1.5 * (n + 1)) - 1 if the input is an odd number. Otherwise n/2</returns>
        protected static CollatzPrimeResult CollatzPrimeResult(ulong n)
        {
            n = Collatz2Quick(n); // the first step is always /2
            var isFirstRowSingle = n % 2 == 0;

            n = Collatz2Quick(n);
            var plusMinus = n % 2 == 0 ? CollatzPlusMinus.PlusOne : CollatzPlusMinus.MinusOne;

            n = Collatz2Quick(n);

            return new CollatzPrimeResult { Result = n, PlusMinus = plusMinus, IsFirstRowSingle = isFirstRowSingle };
        }

        protected static ulong DivideByTwoUntilOdd(ulong n)
        {
            return n % 2 == 1 || n == 0 ? n : DivideByTwoUntilOdd(n / 2);
        }

        protected static CollatzSteps MinusOneDivideByTwoUntilEven(ulong n)
        {
            return MinusOneDivideByTwoUntilEven(new CollatzSteps(n, 0));
        }
        protected static CollatzSteps MinusOneDivideByTwoUntilEven(CollatzSteps steps)
        {
            return steps.Result % 2 == 0 ? steps : MinusOneDivideByTwoUntilEven(new CollatzSteps((steps.Result - 1) / 2, steps.Steps + 1));
        }

        protected static CollatzSteps MinusTwoDivideByThreeWhileIntegerResult(ulong n)
        {
            return MinusTwoDivideByThreeWhileIntegerResult(new CollatzSteps(n, 0));
        }
        protected static CollatzSteps MinusTwoDivideByThreeWhileIntegerResult(CollatzSteps steps)
        {
            return (steps.Result - 2) % 3 == 0 ? MinusTwoDivideByThreeWhileIntegerResult(new CollatzSteps((steps.Result - 2) / 3, steps.Steps + 1)) : steps;
        }

        public static CollatzPrimeInfo CollatzPrimeTriangleInfo(ulong n)
        {
            var oddN = DivideByTwoUntilOdd(n);
            var stepsDown = MinusOneDivideByTwoUntilEven(oddN);
            var totalSteps = MinusTwoDivideByThreeWhileIntegerResult(stepsDown);

            var prime = totalSteps.Result;
            var primeResult = CollatzPrimeResult(prime);

            var steps = totalSteps.Steps;
            if (primeResult.IsFirstRowSingle)
            {
                steps += steps % 2;
                steps /= 2;
            }
            else
            {
                steps -= steps % 2;
                steps /= 2;
            }

            // for each step do n*(n+2)
            var result = primeResult.Result;
            for (int i = 0; i < steps; ++i)
            {
                result = 9 * result + (ulong)primeResult.PlusMinus;
            }

            return new CollatzPrimeInfo { N = n, PrimeResult = primeResult, TotalSteps = totalSteps, Steps = steps, Result = result };
        }

        public static ulong CollatzPrimeTriangleResult(ulong n)
        {
            return CollatzPrimeTriangleInfo(n).Result;

            n = DivideByTwoUntilOdd(n);
            var stepsDown = MinusOneDivideByTwoUntilEven(n);
            var totalSteps = MinusTwoDivideByThreeWhileIntegerResult(stepsDown);

            var prime = totalSteps.Result;
            var primeResult = CollatzPrimeResult(prime);

            var steps = totalSteps.Steps;
            if (primeResult.IsFirstRowSingle)
            {
                steps += steps % 2;
                steps /= 2;
            }
            else
            {
                steps -= steps % 2;
                steps /= 2;
            }

            // for each step do n*(n+2)
            var result = primeResult.Result;
            for (int i = 0; i < steps; ++i)
            {
                result = 9 * result + (ulong)primeResult.PlusMinus;
            }

            return result;
        }

        public static ulong[] CollatzPrimeTriangleResults(ulong prime, int length)
        {
            var results = new ulong[length + 1];
            var primeResult = CollatzPrimeResult(prime);
            var result = primeResult.Result;
            results[0] = result;
            for (int i = 0; i < length; ++i)
            {
                result = 9 * result + (ulong)primeResult.PlusMinus;
                results[i + 1] = result;
            }
            return results;
        }
    }
}
