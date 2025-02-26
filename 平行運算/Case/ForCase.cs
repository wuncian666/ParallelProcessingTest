using CSVHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 平行運算
{
    internal class ForCase
    {
        public ForCase()
        { }

        public void Process(string file, int batchSize, int batchCount)
        {
            Stopwatch totalWatch = Stopwatch.StartNew();
            CSV csv = new CSV();
            for (int i = 0; i < batchCount; i++)
            {
                int count = i;

                Stopwatch sw = Stopwatch.StartNew();

                Console.WriteLine($"第{count + 1}批");

                int current = batchSize * count;
                Console.WriteLine($"開始讀取{current} ~ {current + batchSize}筆資料");

                List<MockData> result = csv.Read<MockData>(file, false, current, batchSize);
                sw.Stop();
                Console.WriteLine($"讀取{current} ~ {current + batchSize}筆資料完成，耗時:{sw.ElapsedMilliseconds / 1000.0f}s");

                //sw.Restart();
                //CSV.Write(file, result, true, true);

                //time += sw.ElapsedMilliseconds / 1000.0f;
                //Console.WriteLine($"寫入{current} ~ {current + batchSize}筆資料完成，耗時:{time}s");
            }
            totalWatch.Stop();
            Console.WriteLine(totalWatch.ElapsedMilliseconds / 1000.0f + "s");
        }
    }
}