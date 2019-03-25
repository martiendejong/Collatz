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
        }
    }
}
