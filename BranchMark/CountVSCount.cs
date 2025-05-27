using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchMark
{
    [MemoryDiagnoser]
    public class CountVSCount
    {
        [Benchmark]
        public void Count()
        {
            var list = new List<int>();
            var count = list.Count;
        }

        [Benchmark]
        public void LinQCount()
        {
            var list = new List<int>();
            var count = list.Count();
        }
    }
}