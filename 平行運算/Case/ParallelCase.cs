using CSVHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 平行運算
{
    public class ParallelCase
    {
        private static readonly object _lockObject = new object();

        private readonly BatchConfig _config;

        public ParallelCase(BatchConfig config)
        {
            _config = config;
        }

        public async Task ProcessAsync()
        {
            //lock mutex concurrentBag concurrentQueueu ReadWriteSlim SemaphoreSlim

            Stopwatch totalWatch = Stopwatch.StartNew();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < _config.BatchCount; i++)
            {
                int count = i;

                var task = new Task(() =>
                {
                    int current = _config.BatchSize * count;
                    string range = $"{current} ~ {current + _config.BatchSize}";
                    Console.WriteLine($"第{count + 1}批開始讀取{range}筆資料");

                    Stopwatch sw = Stopwatch.StartNew();
                    CSV csv = new CSV();
                    List<MockData> result = csv.Read<MockData>(_config.Readpath, true, current, _config.BatchSize);
                    Console.WriteLine($"讀取{range}筆資料完成，耗時:{sw.ElapsedMilliseconds / 1000.0f}s");

                    lock (_lockObject)
                    {
                        sw.Restart();
                        CSV.Write(_config.Writepath, result, true, true);
                        Console.WriteLine($"寫入{range}筆資料完成，耗時:{sw.ElapsedMilliseconds / 1000.0f}s");
                        result.Clear();
                    }
                });

                task.Start();
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            totalWatch.Stop();
            Console.WriteLine($"程式執行結束總耗時:{totalWatch.ElapsedMilliseconds / 1000.0f} s\n");
        }
    }
}