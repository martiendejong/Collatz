using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collatz
{
    public class CollatzSequence
    {
        public CollatzFullSteps Steps;

        public List<CollatzSequence> Children = new List<CollatzSequence>();
    }
}
