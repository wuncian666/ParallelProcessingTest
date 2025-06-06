using CSVHelper;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Parallel平行處理
{
    internal class Program
    {
        // for lock case
        private static readonly object lockObject = new();

        // for read write lock slim case
        private static readonly ReaderWriterLockSlim readWriteLockSlimObject = new();

        private static readonly int _totalSize = 60000000;
        private static readonly int _batchSize = 3500000;

        private static async Task Main()
        {
            BatchConfig config = new(_totalSize, _batchSize);

            await LockProcessAsync(config);
            //await ProcessConcurrentAsync(config);
            //await ReadWriteLockSlimAsync(config);
            //await SemaphoreSlim(config);
        }

        private static async Task LockProcessAsync(BatchConfig config)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<double> readTimes = [];
            List<double> writeTimes = [];
            await Parallel.ForAsync(0, config.BatchCount, async (count, token) =>
            {
                int current = config.BatchSize * count;
                string range = $"{current} ~ {current + config.BatchSize}";
                Console.WriteLine($"第{count + 1}批開始讀取{range}筆資料");

                Stopwatch sw = Stopwatch.StartNew();
                var data = CSVOptimize.ReadOptimize<MockModel>(config.Readpath, current, config.BatchSize);
                sw.Stop();
                readTimes.Add(sw.ElapsedMilliseconds / 1000.0f);
                sw.Restart();
                bool append = count != 0;
                lock (lockObject)
                {
                    CSVOptimize.WriteOptimize<MockModel>(config.Writepath, data, append);
                    sw.Stop();
                    writeTimes.Add(sw.ElapsedMilliseconds / 1000.0f);
                }

                data.Clear();
                data = null;
                GC.Collect();
            });

            stopwatch.Stop();
            Console.WriteLine($"| {_totalSize} | {readTimes.Median():#0.00} | {writeTimes.Median():#0.00} | {stopwatch.ElapsedMilliseconds / 1000.0:#0.00} | 2600 |");
        }

        private static async Task ProcessConcurrentAsync(BatchConfig config)
        {
            ConcurrentBag<MockModel> bag = new();
            Stopwatch stopwatch = Stopwatch.StartNew();

            await Parallel.ForEachAsync(Enumerable.Range(0, config.BatchCount), new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (count, token) =>
            {
                int current = config.BatchSize * count;
                string range = $"{current} ~ {current + config.BatchSize}";
                Console.WriteLine($"第{count + 1}批開始讀取{range}筆資料");

                Stopwatch sw = Stopwatch.StartNew();
                CSV csv = new();
                var data = csv.Read<MockModel>(config.Readpath, false, current, config.BatchSize);
                foreach (var item in data)
                {
                    bag.Add(item);
                }
                Console.WriteLine($"第{count + 1}讀取{range}筆資料完成，耗時:{sw.ElapsedMilliseconds / 1000.0f}s");
            });

            Stopwatch sw = Stopwatch.StartNew();
            CSV.Write(config.Writepath, bag.ToList(), true, true);
            sw.Stop();
            Console.WriteLine($"寫入耗時:{sw.ElapsedMilliseconds / 1000.0f}s");

            stopwatch.Stop();
            Console.WriteLine($"全部任務完成,耗時:{stopwatch.ElapsedMilliseconds / 1000.0f}");
        }

        private static async Task ReadWriteLockSlimAsync(BatchConfig config)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            await Parallel.ForEachAsync(Enumerable.Range(0, config.BatchCount), new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (count, token) =>
            {
                int current = config.BatchSize * count;
                string range = $"{current} ~ {current + config.BatchSize}";

                var data = new List<MockModel>();
                // 讀取操作使用讀鎖
                readWriteLockSlimObject.EnterReadLock();
                try
                {
                    Console.WriteLine($"第{count + 1}批開始讀取{range}筆資料");
                    Stopwatch sw = Stopwatch.StartNew();
                    CSV csv = new();
                    data = csv.Read<MockModel>(config.Readpath, false, current, config.BatchSize);
                    sw.Stop();
                    Console.WriteLine($"第{count + 1}批讀取{range}筆資料完成，耗時:{sw.ElapsedMilliseconds / 1000.0f}s");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    readWriteLockSlimObject.ExitReadLock();
                }

                // 寫入操作使用寫鎖
                readWriteLockSlimObject.EnterWriteLock();
                try
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    CSV.Write(config.Writepath, data, true, true);
                    sw.Stop();
                    Console.WriteLine($"寫入耗時:{sw.ElapsedMilliseconds / 1000.0f}s");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    readWriteLockSlimObject.ExitWriteLock();
                }
            });

            stopwatch.Stop();
            Console.WriteLine($"全部任務完成,耗時:{stopwatch.ElapsedMilliseconds / 1000.0f}");
        }

        private static async Task SemaphoreSlimAsync(BatchConfig config)
        {
            Console.WriteLine($"BatchCount:{config.BatchCount},BatchSize:{config.BatchSize}");
            SemaphoreSlim semaphore = new(1, 1);
            Stopwatch stopwatch = Stopwatch.StartNew();

            await Parallel.ForEachAsync(
                Enumerable.Range(0, config.BatchCount),
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                async (count, token) =>
            {
                int current = config.BatchSize * count;
                string range = $"{current} ~ {current + config.BatchSize}";
                Console.WriteLine($"第{count + 1}批開始讀取{range}筆資料");
                Stopwatch sw = Stopwatch.StartNew();
                CSV csv = new();
                var data = csv.Read<MockModel>(config.Readpath, false, current, config.BatchSize);
                Console.WriteLine($"第{count + 1}批讀取{range}筆資料完成，耗時:{sw.ElapsedMilliseconds / 1000.0f}s");

                await semaphore.WaitAsync(token);
                try
                {
                    sw = Stopwatch.StartNew();
                    CSV.Write(config.Writepath, data, true, true);
                    sw.Stop();
                    Console.WriteLine($"寫入耗時:{sw.ElapsedMilliseconds / 1000.0f}s");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            stopwatch.Stop();
            Console.WriteLine($"全部任務完成,耗時:{stopwatch.ElapsedMilliseconds / 1000.0f}");
        }
    }
}