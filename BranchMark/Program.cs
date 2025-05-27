using BenchmarkDotNet.Running;
using BranchMark;
using Iced.Intel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BenchMark
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Count VS Count()
            var summary = BenchmarkRunner.Run<Read_VS_OptimizeRead>();

            //string input = "18,Josias,Consterdine,jconsterdineh@scribd.com,Male,149.190.102.195";

            //StringBuilder builder = new StringBuilder(90);
            //builder.Append(input);

            //char[] chars = new char[90];

            //builder.CopyTo(0, chars, 0, builder.Length - 1);

            //StreamWriter writer = new StreamWriter("data.csv");
            //writer.WriteLine(chars, 0, builder.Length - 1);
            //writer.Flush();
            //writer.Close();
        }
    }
}