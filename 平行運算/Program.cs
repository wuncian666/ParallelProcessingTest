using CSVHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 平行運算
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            BatchConfig config = new BatchConfig(10000000, 3500000);
            if (Directory.Exists(@"C:\Users\wuncian\source\repos\平行運算\WriteData"))
            {
                Directory.Delete(@"C:\Users\wuncian\source\repos\平行運算\WriteData", true);
            }

            //ForCase forCase = new ForCase();
            //forCase.Process(file, batchSize, batchCount);
            ParallelCase parallelCase = new ParallelCase();
            await parallelCase.ProcessAsync();
            //LineByLineCase lineByLineCase = new LineByLineCase();
            //lineByLineCase.Process(file, batchSize);

            Console.ReadKey();
        }
    }
}