using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Collatz
{





    class Program
    {
        // returns 
        // the result (1)
        // number of steps (2)
        // base number (3)
        // number of steps from base number (4)
        public static Tuple<ulong, int, ulong, int, ulong, int> CollatzM(ulong n)
        {
            ulong m = n;
            int steps = 0;

            while ((m - 1) % 2 == 0)
            {
                m = (m - 1) / 2;
                steps++;
            }

            var b = m;
            var steps2 = steps;
            while ((b - 2) % 3 == 0)
            {
                b = (b - 2) / 3;
                steps2++;
            }

            m = Collatz2(m);
            m = Collatz2(m);

            var quadrantBase = m;

            int s = steps + 1;
            s = s - (s % 2);
            s = s / 2;

            var quadrantSteps = s;

            for (int i = 0; i < s; ++i)
            {
                m = 9 * m + 2;
            }


            return new Tuple<ulong, int, ulong, int, ulong, int>(m, steps, b, steps2, quadrantBase, quadrantSteps);
        }

        // 1 the resulting number
        // 2 the diagonalStart
        // 3 the diagonalIntermediateResult
        // 4 the diagonalStartResult
        // 5 the number of times *9+2
        public static Tuple<ulong, ulong, ulong, ulong, int> CollatzDiagonalResult(ulong n)
        {
            // divide n by two until we get an odd number
            while (n > 0 && n % 2 == 0)
            {
                n = n / 2;
            }

            // get the number on the diagonal line
            // by doing (n-1)/2 until we reach an even number
            int steps = 0;
            ulong numberOnDiagonal = n;
            while ((numberOnDiagonal - 1) % 2 == 0)
            {
                numberOnDiagonal = (numberOnDiagonal - 1) / 2;
                steps++;
            }

            // get the number at the start of the diagonal
            // by doing (n-2)/3 as long as we get a whole number
            var diagonalStart = numberOnDiagonal;
            while ((diagonalStart - 2) % 3 == 0)
            {
                diagonalStart = (diagonalStart - 2) / 3;
                steps++;
            }

            // calculate the result of the starting number
            // by doing 3*(n+1)/2 for odd and 2/n for an even number
            var diagonalIntermediate = Collatz2(diagonalStart);
            if (diagonalIntermediate % 2 == 0)
            {
                steps++;
            }
            var diagonalResult = Collatz2(diagonalIntermediate);

            // the steps are calculated as follows:
            // each step upwards from n by doing (n-1)/2 counts as one step
            // each step diagonally by doing (n-2)/3 counts as one step
            // if the last step is n/2 then we can add one more step
            // divide steps by two rounded down
            steps = steps - (steps % 2);
            steps = steps / 2;

            // for each step do n*(n+2)
            var result = diagonalResult;
            for (int i = 0; i < steps; ++i)
            {
                result = 9 * result + 2;
            }

            return new Tuple<ulong, ulong, ulong, ulong, int>(result, diagonalStart, diagonalIntermediate, diagonalResult, steps);
        }

        public static void WriteText(string text, int length)
        {
            Console.Write(text + new string(' ', length - text.Count()));
            sb.Append(text + new string(' ', length - text.Count()));
        }

        public static void WriteLine()
        {
            Console.WriteLine();
            sb.AppendLine();
        }

        public static StringBuilder sb = new StringBuilder();

        public static string[] GetPrimeList(ulong prime, int length)
        {
            var results = new string[length + 1];
            results[0] = $"[{prime}]";
            var list = CollatzCalculator.CollatzPrimeTriangleResults(prime, 10);
            for (var j = 0; j < 10; ++j)
            {
                var n = list[j];
                var info = CollatzCalculator.CollatzPrimeTriangleInfo(n);
                var nextPrime = info.TotalSteps.Result;
                var steps = info.Steps;

                results[j + 1] = $@"""{n} => {nextPrime}[{steps}] {info.Result}""";
            }
            return results;
        }

        static void Main(string[] args)
        {
            var path = @"d:\Projects\Collatz\primes.csv";

            var results = new string[200][];

            ulong prime = 0;
            for(var i = 0; i < 200; ++ i)
            {
                results[i] = GetPrimeList(prime, 10);

                ++i;
                prime += 4;
                results[i] = GetPrimeList(prime, 10);

                prime += 2;
            }

            using (StreamWriter sw = new StreamWriter(File.OpenWrite(path)))
            {
                for (var i = 0; i < 11; ++i)
                {
                    List<string> row = new List<string>();
                    row.Add(i == 0 ? @"""""": (i - 1).ToString());
                    for (var j = 0; j < 200; ++j)
                    {
                        row.Add(results[j][i]);
                    }
                    sw.WriteLine(string.Join(",", row));
                }
                sw.Close();
            }

            return;

            ulong rowN = 1;
            for (ulong i = 1; i < 10; ++i)
            {
                rowN = rowN * 9 + 1;
                var n = rowN;
                WriteText(n.ToString(), 10);
                while (n >= rowN)
                {
                    var result = CollatzDiagonalResult(n);
                    n = result.Item1;
                    WriteText(n.ToString(), 10);
                }
                WriteLine();
            }
            File.WriteAllText(@"e:\collatz10.txt", sb.ToString());
            return;

            for (ulong i = 1; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    var n = i;
                    for (int k = 0; k < j; ++k)
                    {
                        n = n * 9 + 2;
                    }

                    var steps = new List<ulong>();
                    ulong z = 99, o = n;
                    while (z > 10)
                    {
                        var m = CollatzDiagonalResult(o);
                        o = m.Item1;
                        steps.Add(o);

                        z = o;
                        while ((z - 2) % 9 == 0)
                        {
                            z = (z - 2) / 9;
                        }
                    }
                    WriteText(steps.Count().ToString(), 5);
                    WriteText(n + " => " + o + " (" + z + ")", 40);
                }
                WriteLine();
            }
            File.WriteAllText(@"e:\collatz9.txt", sb.ToString());
        }

        public static void tree()
        {
            var tree = new Dictionary<ulong, List<ulong>>();
            var cache = new List<ulong>();

            ulong n = 511;
            for (ulong i = 1; i < 10000; ++i)
            {
                n = i;
                var first = true;
                //WriteText(n + " => ", 10);
                while (n > 2)//(!cache.Contains(n))
                {
                    //cache.Add(n);


                    var result = CollatzDiagonalResult(n);

                    var m = result.Item1;
                    if (!first)
                    {
                        if (!tree.ContainsKey(m))
                            tree.Add(m, new List<ulong>());
                        if (!tree[m].Contains(n))
                            tree[m].Add(n);
                    }
                    else
                    {
                        first = false;
                    }

                    n = m;
                    /*if(cache.Contains(n))
                    {
                        WriteText(n.ToString(), 20);
                        break;
                    }*/

                    var diagonalStart = result.Item2;
                    var diagonalIntermediate = result.Item3;
                    var diagonalResult = result.Item4;
                    var steps = result.Item5;

                    //WriteText(diagonalResult + "," + steps + "(" + diagonalStart + "," + diagonalIntermediate + ")= " + n, 40);

                    //WriteText("(" + diagonalResult + "," + steps + ")", 15);
                    //WriteText(n.ToString(), 10);



                    /*Console.WriteLine("Starting number:           " + n);
                    /*Console.WriteLine("Number of steps:           " + steps);
                    Console.WriteLine("Base number:               " + baseN);
                    Console.WriteLine("Number of steps from base: " + stepsFromBase);*/
                    /*Console.WriteLine("Quadrant base:             " + quadrantBase);
                    Console.WriteLine("Quadrant steps:            " + quadrantSteps);
                    Console.WriteLine("Resulting number:          " + m);*/

                    //Console.WriteLine();


                }

                //WriteLine();
            }
            //File.WriteAllText(@"e:\collatz6.txt", sb.ToString());

            for (int i = 0; i < 5000; ++i)
            {
                foreach (var node in tree
                    .Where(node =>
                     {
                         var nr = node.Key;
                         while ((nr - 2) % 9 == 0)
                         {
                             nr = (nr - 2) / 9;
                         }
                         return nr == (ulong)i;
                     })
                    .OrderBy(node => node.Key))
                {
                    if (node.Value.Count() == 0)
                        break;
                    var baseN = node.Key;
                    var steps = 0;
                    while ((baseN - 2) % 9 == 0)
                    {
                        baseN = (baseN - 2) / 9;
                        steps++;
                    }
                    WriteText(node.Key + " (" + baseN + "," + steps + ") <= ", 25);
                    foreach (var value in node.Value.OrderBy(value => value))
                    {
                        if (value % 2 == 0)
                            continue;

                        baseN = value;
                        steps = 0;
                        while ((baseN - 2) % 9 == 0)
                        {
                            baseN = (baseN - 2) / 9;
                            steps++;
                        }
                        WriteText(value + " (" + baseN + "," + steps + ")", 25);
                        //WriteText(value.ToString(), 10);
                    }
                    WriteLine();
                }
            }
            File.WriteAllText(@"e:\collatz8.txt", sb.ToString());
            while (n > 10) ;
            // test if the result is correct
            /*ulong r = n;
            for (int i = 0; i < steps + 2; ++i)
            {
                r = Collatz2(r);
            }*/

            //Console.WriteLine(m);
            //Console.WriteLine(r);



            /*for (ulong k = 0; k <= 10; k++)
            {
                for (ulong i = (100 * k) + 3; i <= 100 * (k + 1); i = i * 2 + 1)
                {*/


            //var j = 0;
            //while (j < 1000000000000) ;







            //WriteSteps()


            //WriteCollatz2();

            //WriteTree();
            //return;

        }

        public delegate bool fn(int n);

        public static void WriteTree()
        {
            var tree = CollatzFullSteps.BuildSequenceTree(100000, 3, 4);

            var level = 1;
            List<int> iterators = new List<int>();
            CollatzSequence leaf = null;

            var columnWidth = 40;

            var sb = new StringBuilder();
            while ((leaf != tree) || iterators.Count > 0)
            {
                leaf = tree;
                var parent = leaf;
                for (int i = 0; i < iterators.Count(); ++i)
                {
                    parent = leaf;
                    leaf = leaf.Children[iterators[i]];
                }

                var text = " <=(" + leaf.Steps.NumberOfOddSteps + "," + leaf.Steps.NumberOfEvenSteps + ")= " + ToBase(leaf.Steps.Number, 2);
                var size = text.Count();

                //if(leaf.Steps.NumberOfOddSteps > 1)
                sb.Append(text + new string(' ', columnWidth - size));
                //Console.Write(" <=(" + leaf.Steps.NumberOfOddSteps + "," + leaf.Steps.NumberOfEvenSteps + ")= " + leaf.Steps.Number + "\t");

                if (leaf.Children.Any()/* && iterators.Count < 10*/)
                {
                    iterators.Add(0);
                }
                else
                {
                    while (iterators.Count > 0 && iterators[iterators.Count - 1] >= parent.Children.Count - 1)
                    {
                        iterators.RemoveAt(iterators.Count - 1);
                        //iterators[iterators.Count - 1]++;

                        leaf = tree;
                        parent = leaf;
                        for (int i = 0; i < iterators.Count(); ++i)
                        {
                            parent = leaf;
                            leaf = leaf.Children[iterators[i]];
                        }
                    }

                    if (iterators.Count > 0)
                    {
                        iterators[iterators.Count - 1]++;
                    }

                    sb.AppendLine();
                    //Console.WriteLine();
                    for (int i = 0; i < iterators.Count(); ++i)
                    {
                        sb.Append(new string(' ', columnWidth));
                    }
                }
            }
            File.WriteAllText(@"e:\collatz.txt", sb.ToString());
        }

        public static void WriteCollatz2()
        {
            var line = 1;
            ulong n = 1;
            var sb = new StringBuilder();
            var columnWidth = 25;
            while (line < 11)
            {
                var text = n + " => ";
                sb.Append(text + new string(' ', columnWidth - text.Count()));
                var result = n;
                while (result != 1)
                {
                    if (result % 2 == 0)
                        result = result / 2;
                    else
                        result = (ulong)(result * 1.5 + 0.5);

                    text = result + " => ";
                    sb.Append(text + new string(' ', columnWidth - text.Count()));
                }

                n = n * 2 + 1;
                line++;
                sb.AppendLine();
            }
            File.WriteAllText(@"e:\collatz2.txt", sb.ToString());
        }

        public static void WriteSteps()
        {
            var cache2 = new List<ulong>();
            var sb = new StringBuilder();
            for (ulong i = 1; i < 10000000000; i = i * 2 + 1)
            {
                //var result = CollatzSteps.GetTotalSteps(i);
                var result = CollatzFullSteps.GetStepsUntilSmallerOrCached(i, cache2);

                //if (result.Count > 2)
                //{
                if (result.Count > 0)
                {
                    sb.AppendLine(i + string.Join("", result.Select(steps =>
                    {
                        var text = " => (" + steps.NumberOfOddSteps + "," + steps.NumberOfEvenSteps + ") " + steps.Result;
                        return text + new string(' ', 30 - text.Count());
                    })));
                    //Console.WriteLine(ToBase(i, 2) + string.Join("", result.Select(steps => " => (" + steps.NumberOfOddSteps + "," + steps.NumberOfEvenSteps + ") " + ToBase(steps.Result, 2))));
                    //Console.WriteLine(i + " => (" + result.First().NumberOfOddSteps + "," + result.First().NumberOfEvenSteps + ")\t" + result.First().Result + " (" + result.Count + ")");
                    //Console.WriteLine(i + " => (" + result.First().NumberOfOddSteps + "," + result.First().NumberOfEvenSteps + ")\t" + result.First().Result + string.Join("", result.Skip(1).Select(steps => " => (" + (steps.Result > steps.Number ? "+" : "") + (steps.Result - steps.Number) + ") " + steps.Result)));
                    //Console.Write(i + " in " + result.Count + " steps");
                    //Console.WriteLine("\tlast: " + result.Last().Result + " \t => (" + result.First().NumberOfOddSteps + ", " + result.First().NumberOfEvenSteps + ")\t" + result.First().Result + " " + string.Join(" ", result.Skip(1).Select(steps => steps.Result)));
                }
                //}
                //}
                //Console.WriteLine("----- cache count " + cache2.Count + " -----");

                //var result = CollatzSteps.GetTotalSteps(i);
                //Console.WriteLine(i + string.Join("", result.Select(steps => " => (" + steps.NumberOfOddSteps + "," + steps.NumberOfEvenSteps + ")" + steps.Result)));

                //var steps = CollatzSteps.GetSteps(i);
                //Console.WriteLine(i + string.Join("", "\t=> (\t" + steps.NumberOfOddSteps + ",\t" + steps.NumberOfEvenSteps + " )\t" + steps.Result));

            }
            File.WriteAllText(@"e:\collatz3.txt", sb.ToString());
        }










        public static void zooi()
        {
            var cache2 = new List<ulong>();

            for (ulong k = 0; k <= 10; k++)
            {
                for (ulong i = (100 * k) + 3; i <= 100 * (k + 1); i += 4)
                {
                    //var result = CollatzSteps.GetTotalSteps(i);
                    var result = CollatzFullSteps.GetStepsUntilSmallerOrCached(i, cache2);

                    //if (result.Count > 2)
                    //{
                    if (result.Count > 0)
                    {
                        Console.WriteLine(ToBase(i, 2) + string.Join("", result.Select(steps => " => (" + steps.NumberOfOddSteps + "," + steps.NumberOfEvenSteps + ") " + ToBase(steps.Result, 2))));
                        //Console.WriteLine(i + " => (" + result.First().NumberOfOddSteps + "," + result.First().NumberOfEvenSteps + ")\t" + result.First().Result + " (" + result.Count + ")");
                        //Console.WriteLine(i + " => (" + result.First().NumberOfOddSteps + "," + result.First().NumberOfEvenSteps + ")\t" + result.First().Result + string.Join("", result.Skip(1).Select(steps => " => (" + (steps.Result > steps.Number ? "+" : "") + (steps.Result - steps.Number) + ") " + steps.Result)));
                        //Console.Write(i + " in " + result.Count + " steps");
                        //Console.WriteLine("\tlast: " + result.Last().Result + " \t => (" + result.First().NumberOfOddSteps + ", " + result.First().NumberOfEvenSteps + ")\t" + result.First().Result + " " + string.Join(" ", result.Skip(1).Select(steps => steps.Result)));
                    }
                    //}
                }
                Console.WriteLine("----- cache count " + cache2.Count + " -----");

                //var result = CollatzSteps.GetTotalSteps(i);
                //Console.WriteLine(i + string.Join("", result.Select(steps => " => (" + steps.NumberOfOddSteps + "," + steps.NumberOfEvenSteps + ")" + steps.Result)));

                //var steps = CollatzSteps.GetSteps(i);
                //Console.WriteLine(i + string.Join("", "\t=> (\t" + steps.NumberOfOddSteps + ",\t" + steps.NumberOfEvenSteps + " )\t" + steps.Result));

            }
            var j = 0;
            while (j < 1000000000000) ;
            return;




            var n = 3;
            var cache = new List<int>();
            CollatzResult longest = null;

            fn fnCond1 = i => (i + 1) % 3 == 0; // als waar dan is i het gevolg van een lager getal
            fn fnCond2 = i =>
            {
                var x = CollatzQuick(CollatzQuick(i)); // als na 4 iteraties x % 4 == 0 dan is het resultaat < i
                return (x % 4 == 0 && x / 4 < i) || (x % 8 == 0 && x / 8 < i);
            };
            fn fnCond3 = i =>
            {
                var x = (i * 4) - 1;
                return x % 3 == 0 ? (((x / 3) * 2) - 1) % 3 == 0 : false;
            }; // als waar dan is i het gevolg van een lager getal
            fn fnCond4 = i =>
            {
                var x = CollatzQuick(CollatzQuick(CollatzQuick(i))); // als na 4 iteraties x % 4 == 0 dan is het resultaat < i
                return (x % 8 == 0 && x / 8 < i);
            };
            fn fnCond5 = i =>
            {
                var x = CollatzQuick(CollatzQuick(i));
                if (x % 2 == 0)
                {
                    x = CollatzQuick(x / 2);
                    return x % 2 == 0 && x / 2 < i;
                }
                return false;
            };
            fn fnCond6 = i =>
            {
                var x = CollatzQuick(CollatzQuick(CollatzQuick(i))); // als na 6 iteraties x % 4 == 0 dan is het resultaat < i
                return (x % 4 == 0 && x / 4 < i);
            };

            while (n < 1000)
            {
                if (!(fnCond1(n) || fnCond2(n) || fnCond3(n) || fnCond4(n) || fnCond5(n) || fnCond6(n)))
                {
                    var result = CollatzOddSequenceAndLengthWithCache(n, /*new List<int>()*/cache);
                    if (longest == null || result.Length > longest.Length) longest = result;
                    //var numbers = CollatzSequence(n);
                    //Console.WriteLine(CollatzQuick(n));
                    //Console.WriteLine(CollatzQuick(CollatzQuick(CollatzQuick(n))));
                    Console.WriteLine(n + " => " + string.Join(", ", result.Numbers.Select(number => (number + 1) % 4 == 0 && !(fnCond1(number) || fnCond2(number) || fnCond3(number) || fnCond4(number) || fnCond5(number) || fnCond6(number)) ? "[" + number.ToString() + "]" : number.ToString())));
                    //Console.WriteLine(ToBase4(n) + " => " + string.Join(", ", numbers.Select(number => ToBase4(number).ToString())));
                    //Console.WriteLine(ToBase(n, 16, ".") + " => " + string.Join(", ", numbers.Select(number => ToBase(number, 16, ".").ToString())));
                }
                n += 4;
            }
            Console.WriteLine(n + " => " + string.Join(", ", longest.Numbers.Select(number => (number + 1) % 4 == 0 && !(fnCond1(number) || fnCond2(number) || fnCond3(number) || fnCond4(number) || fnCond5(number) || fnCond6(number)) ? "[" + number.ToString() + "]" : number.ToString())));

            while (n < 10000) ;
        }



        public static int ToBase4(int n)
        {
            var base4 = 0;
            var i = 0;


            while (n > Math.Pow(4, i - 1))
            {
                var amount = n % (int)Math.Pow(4, i + 1);
                base4 += (amount / (int)Math.Pow(4, i)) * (int)Math.Pow(10, i);
                ++i;
                n -= amount;
            }

            return base4;
        }

        public static string ToBase(ulong n, int toBase, string separator = "")
        {
            var baseX = "";
            var i = 0;

            while (n > Math.Pow(toBase, i - 1))
            {
                var amount = n % (ulong)Math.Pow(toBase, i + 1);
                baseX = (amount / (ulong)Math.Pow(toBase, i)) + separator + baseX;
                ++i;
                n -= amount;
            }

            return baseX;
        }

        public static CollatzResult CollatzOddSequenceAndLengthWithCache(int n, List<int> cache)
        {
            CollatzResult result = new CollatzResult();
            result.N = n;
            result.Numbers = new List<int>();

            if (cache.Contains(n)) return result;

            result.Numbers.Add(CollatzOdd(n));
            result.Length = 1;

            while (result.Numbers.Last() > n && !cache.Contains(result.Numbers.Last()))
            {
                result.Numbers.Add(CollatzOdd(result.Numbers.Last()));
                result.Length++;
            }

            cache.AddRange(result.Numbers);

            return result;
        }

        public static List<int> CollatzOddSequenceWithCache(int n, List<int> cache)
        {
            List<int> numbers = new List<int>();

            if (cache.Contains(n)) return numbers;

            numbers.Add(CollatzOdd(n));

            while (numbers.Last() > n && !cache.Contains(numbers.Last()))
            {
                numbers.Add(CollatzOdd(numbers.Last()));
            }

            cache.AddRange(numbers);

            return numbers;
        }

        public static List<int> CollatzSequence(int n)
        {
            List<int> numbers = new List<int>() { n };

            do
            {
                numbers.Add(Collatz(numbers.Last()));
            }
            while (numbers.Last() > numbers.First());

            return numbers;
        }

        public static int CollatzQuick(int n)
        {
            return (int)(1.5 * (n + 1)) - 1;
            //return n + ((n + 1) / 2);
        }

        public static int CollatzOdd(int n)
        {
            n = Collatz(n);
            while (n % 2 == 0)
                n /= 2;
            return n;
        }

        public static int Collatz(int n)
        {
            return n % 2 == 0 ? n / 2 : n * 3 + 1;
        }

        public static ulong Collatz2(ulong n)
        {
            return n % 2 == 0 ? n / 2 : (n * 3 + 1) / 2;
        }
    }

    public class CollatzSteps
    {
        public readonly ulong Result;

        public readonly int Steps;

        public  CollatzSteps(ulong result, int steps)
        {
            Result = result;
            Steps = steps;
        }
    }

    public enum CollatzPlusMinus
    {
        PlusOne = 1,
        MinusOne = -1
    }

    public class CollatzPrimeResult
    {
        public ulong Result;
        public CollatzPlusMinus PlusMinus;
        public bool IsFirstRowSingle;
    }

    public class CollatzPrimeInfo
    {
        public ulong N;
        public ulong Result;
        public int Steps;
        public CollatzSteps TotalSteps;
        public CollatzPrimeResult PrimeResult;
    }

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
            return n % 2 == 1 || n == 0 ? n : DivideByTwoUntilOdd(n/2);
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
