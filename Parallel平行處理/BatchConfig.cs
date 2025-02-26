using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallel平行處理
{
    public class BatchConfig
    {
        public int BatchSize { set; get; }
        public int TotalSzie { set; get; }
        public int BatchCount { set; get; }
        public string Readpath { set; get; }
        public string Writepath { set; get; }

        public BatchConfig(int totalSize, int batchSize)
        {
            this.TotalSzie = totalSize;
            this.BatchSize = batchSize;
            this.BatchCount = totalSize / batchSize + 1;
            this.Readpath = $@"C:\Users\wuncian\source\repos\平行運算\MockData\MOCK_DATA_{totalSize}筆.csv";
            this.Writepath = $@"C:\Users\wuncian\source\repos\平行運算\WriteDatas\MOCK_DATA_{totalSize}筆.csv";
        }
    }
}