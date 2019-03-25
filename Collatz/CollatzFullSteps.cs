using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collatz
{
    public class CollatzFullSteps
    {
        public ulong Number;

        public int NumberOfOddSteps;

        public int NumberOfEvenSteps;

        public ulong Result;

        public static List<CollatzFullSteps> GetTotalSteps(ulong number)
        {
            var result = new List<CollatzFullSteps>()
            {
                GetSteps(number)
            };

            while (result.Last().Result != 1)
            {
                result.Add(GetSteps((result.Last().Result)));
            }

            return result;
        }

        public static CollatzSequence BuildSequence(ulong number, Dictionary<ulong, CollatzSequence> cache)
        {
            if (cache.ContainsKey(number))
            {
                return cache[number];
            }

            var sequence = new CollatzSequence();
            cache.Add(number, sequence);
            sequence.Steps = GetSteps(number);

            if (!cache.ContainsKey(sequence.Steps.Result))
            {
                BuildSequence(sequence.Steps.Result, cache);
            }

            if (sequence.Steps.Result != number)
            {
                cache[sequence.Steps.Result].Children.Add(sequence);
            }

            return sequence;
        }

        public static CollatzSequence BuildSequenceTree(ulong maxNumber, ulong startNumber = 1, ulong steps = 1)
        {
            var sequences = new Dictionary<ulong, CollatzSequence>();
            for (ulong number = startNumber; number <= maxNumber; number = number + steps)
            {
                if (!sequences.ContainsKey(number))
                {
                    BuildSequence(number, sequences);
                }
            }
            return sequences[1];
        }

        public static List<CollatzFullSteps> GetStepsUntilSmallerOrCached(ulong number, List<ulong> cache)
        {
            cache.RemoveAll(n => n < number);

            var result = new List<CollatzFullSteps>();
            if (!cache.Contains(number))
            {
                CollatzFullSteps steps;
                steps = GetSteps(number);
                result.Add(steps);

                while (steps.Result > number)
                {
                    if (cache.Contains(steps.Result) || number > steps.Result)
                    {
                        break;
                    }
                    else
                    {
                        cache.Add(steps.Result);
                    }

                    steps = GetSteps(steps.Result);
                    result.Add(steps);
                }
            }
            return result;
        }

        public static List<CollatzFullSteps> GetStepsUntilSmaller(ulong number)
        {
            var result = new List<CollatzFullSteps>()
            {
                GetSteps(number)
            };

            while (result.Last().Result > number)
            {
                result.Add(GetSteps((result.Last().Result)));
            }

            return result;
        }

        public static CollatzFullSteps GetSteps(ulong number)
        {
            var oddSteps = GetOddSteps(number);
            var evenSteps = GetEvenSteps(oddSteps.Item1);

            return new CollatzFullSteps()
            {
                Number = number,
                NumberOfOddSteps = oddSteps.Item2,
                NumberOfEvenSteps = evenSteps.Item2,
                Result = evenSteps.Item1
            };
        }

        /*public int DetermineNumberOfSteps(ulong oddNumber)
        {
            throw new methodno
        }*/

        public static Tuple<ulong, int> GetOddSteps(ulong oddNumber)
        {
            int steps = 0;
            while (oddNumber % 2 == 1)
            {
                ++steps;
                oddNumber = (ulong)(1.5 * (oddNumber + 1)) - 1;
            }
            return new Tuple<ulong, int>(oddNumber, steps);
        }

        public static Tuple<ulong, int> GetEvenSteps(ulong evenNumber)
        {
            int steps = 0;
            while (evenNumber % 2 == 0)
            {
                ++steps;
                evenNumber /= 2;
            }
            return new Tuple<ulong, int>(evenNumber, steps);
        }
    }

}
