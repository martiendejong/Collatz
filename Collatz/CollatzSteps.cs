using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collatz
{
    public class CollatzSteps
    {
        public readonly ulong Result;

        public readonly int Steps;

        public CollatzSteps(ulong result, int steps)
        {
            Result = result;
            Steps = steps;
        }
    }
}
