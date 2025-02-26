using CSVHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 平行運算
{
    public class LineByLineCase
    {
        public LineByLineCase()
        { }

        // List<MockData> result1, result2, result3 會導致變慢，只有一個result 不會
        public void Process(string file, int batchSize)
        {
            Stopwatch swtotal = Stopwatch.StartNew();
            List<MockData> result = null;
            Stopwatch sw = Stopwatch.StartNew();
            int current = 0;
            CSV csv = new CSV();
            Console.WriteLine($"開始讀取1筆資料");
            result = csv.Read<MockData>(file, false, current, batchSize);
            //List<MockData> result1 = csv.Read<MockData>(file, false, current, batchSize);
            sw.Stop();
            Console.WriteLine($"讀取1筆資料完成，耗時:{sw.ElapsedMilliseconds / 1000.0f}s");
            //result1.Clear();
            //GC.Collect();

            sw.Restart();
            current += batchSize;
            Console.WriteLine($"開始讀取2筆資料");
            result = csv.Read<MockData>(file, false, current, batchSize);
            //List<MockData> result2 = csv.Read<MockData>(file, false, current, batchSize);
            sw.Stop();
            Console.WriteLine($"讀取1筆資料完成，耗時:{sw.ElapsedMilliseconds / 1000.0f}s");
            //result2.Clear();
            //GC.Collect();

            sw.Restart();
            current += batchSize;
            Console.WriteLine($"開始讀取3筆資料");
            result = csv.Read<MockData>(file, false, current, batchSize);
            //List<MockData> result3 = csv.Read<MockData>(file, false, current, batchSize);
            sw.Stop();
            Console.WriteLine($"讀取3筆資料完成，耗時:{sw.ElapsedMilliseconds / 1000.0f}s");
            //result3.Clear();
            //GC.Collect();

            swtotal.Stop();
            Console.WriteLine("程式執行結束");
            Console.WriteLine($"總耗時:{swtotal.ElapsedMilliseconds / 1000.0f} s\n");
        }
    }
}