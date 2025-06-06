namespace Parallel平行處理
{
    public class BatchConfig(int totalSize, int batchSize)
    {
        public int BatchSize { set; get; } = batchSize;
        public int TotalSzie { set; get; } = totalSize;
        public int BatchCount { set; get; } = (totalSize % batchSize) == 0 ? (totalSize / batchSize) : (totalSize / batchSize) + 1;
        public string Readpath { set; get; } = $@"D:\csv-process\csv-mock-data\MOCK_DATA_{totalSize}筆.csv";
        public string Writepath { set; get; } = $@"D:\csv-process\csv-write-data\MOCK_DATA_{totalSize}筆.csv";
    }
}