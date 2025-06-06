
## 模擬CSV讀取+反射
| Method       | Mean        | Error     | StdDev    | Gen0   | Allocated |
|------------- |------------:|----------:|----------:|-------:|----------:|
| Read         | 887.4563 ns | 7.6859 ns | 6.8134 ns | 0.1640 |     861 B |
| OptimizeRead |   0.0536 ns | 0.0211 ns | 0.0197 ns |      - |         - |

## 模擬CSV讀取+反射 (350萬筆)
| Method | Mean    | Error    | StdDev   | Gen0        | Gen1        | Gen2      | Allocated |
|------- |--------:|---------:|---------:|------------:|------------:|----------:|----------:|
| Read   | 5.177 s | 0.0236 s | 0.0197 s | 458000.0000 | 449000.0000 | 9000.0000 |   2.67 GB |

## SPLIT vs SPAN
| Method       | Mean     | Error   | StdDev  | Gen0   | Allocated |
|------------- |---------:|--------:|--------:|-------:|----------:|
| Read         | 154.0 ns | 2.45 ns | 2.51 ns | 0.1047 |     549 B |
| OptimizeRead | 250.8 ns | 3.82 ns | 3.57 ns | 0.0477 |     252 B |

## SPLIT vs SPAN 同時做反射的對比
| Method       | Mean       | Error   | StdDev  | Gen0   | Allocated |
|------------- |-----------:|--------:|--------:|-------:|----------:|
| Read         |   899.0 ns | 4.88 ns | 4.33 ns | 0.1640 |     861 B |
| OptimizeRead | 1,012.0 ns | 8.12 ns | 7.60 ns | 0.1068 |     565 B |

# 讀取 反射
| Method       | Mean     | Error   | StdDev  | Gen0   | Allocated |
|------------- |---------:|--------:|--------:|-------:|----------:|
| Read         | 899.1 ns | 8.56 ns | 8.01 ns | 0.1640 |     861 B |
| OptimizeRead | 915.4 ns | 8.79 ns | 7.34 ns | 0.1001 |     529 B |

# 讀取 反射只做一次
| Method       | Mean     | Error    | StdDev  | Gen0   | Allocated |
|------------- |---------:|---------:|--------:|-------:|----------:|
| Read         | 904.2 ns | 11.12 ns | 9.86 ns | 0.1640 |     861 B |
| OptimizeRead | 327.4 ns |  5.89 ns | 5.51 ns | 0.0639 |     336 B |

# 讀取 反射只做一次 少一個陣列和迴圈
| Method       | Mean     | Error    | StdDev   | Gen0   | Allocated |
|------------- |---------:|---------:|---------:|-------:|----------:|
| Read         | 900.1 ns | 17.40 ns | 19.34 ns | 0.1640 |     861 B |
| OptimizeRead | 312.8 ns |  3.77 ns |  3.53 ns | 0.0572 |     300 B |

# 寫入 反射只做一次
| Method        | Mean     | Error    | StdDev   | Gen0   | Allocated |
|-------------- |---------:|---------:|---------:|-------:|----------:|
| Write         | 713.0 ns | 14.08 ns | 31.79 ns | 0.1383 |     729 B |
| OptimizeWrite | 232.8 ns |  0.94 ns |  0.84 ns | 0.1321 |     693 B |

# 寫入 反射只做一次 StringBuilder
| Method        | Mean     | Error    | StdDev   | Gen0   | Allocated |
|-------------- |---------:|---------:|---------:|-------:|----------:|
| Write         | 677.8 ns | 10.93 ns | 10.22 ns | 0.1383 |     729 B |
| OptimizeWrite | 235.6 ns |  1.55 ns |  1.29 ns | 0.1092 |     573 B |

# 寫入 反射只做一次 StringBuilder + StringBuilder拆開
| Method        | Mean     | Error    | StdDev   | Gen0   | Allocated |
|-------------- |---------:|---------:|---------:|-------:|----------:|
| Write         | 684.9 ns | 13.56 ns | 16.15 ns | 0.1383 |     729 B |
| OptimizeWrite | 154.0 ns |  0.73 ns |  0.65 ns | 0.0663 |     349 B |

# 寫入 反射只做一次 StringBuilder + StringBuilder拆開 + 改用判斷取代 string.trimend
| Method        | Mean     | Error   | StdDev  | Gen0   | Allocated |
|-------------- |---------:|--------:|--------:|-------:|----------:|
| Write         | 682.3 ns | 8.19 ns | 6.84 ns | 0.1383 |     729 B |
| OptimizeWrite | 117.7 ns | 1.36 ns | 1.27 ns | 0.0343 |     180 B |

# 寫入 反射只做一次 StringBuilder + StringBuilder拆開 + 改用判斷取代 string.trimend 
# + 用char[]取代string
| Method        | Mean     | Error   | StdDev  | Gen0   | Allocated |
|-------------- |---------:|--------:|--------:|-------:|----------:|
| Write         | 681.7 ns | 7.37 ns | 6.53 ns | 0.1383 |     729 B |
| OptimizeWrite | 115.2 ns | 0.68 ns | 0.57 ns | 0.0061 |      32 B |

# 寫入 反射只做一次 StringBuilder + StringBuilder拆開 + 改用判斷取代 string.trimend 
# + 用char[]取代string 350萬筆
| Method        | Mean       | Error    | StdDev   | Gen0        | Allocated  |
|-------------- |-----------:|---------:|---------:|------------:|-----------:|
| Write         | 2,393.1 ms | 23.91 ms | 21.20 ms | 486000.0000 | 2433.54 MB |
| OptimizeWrite |   406.1 ms |  3.98 ms |  3.72 ms |  21000.0000 |  106.97 MB |


# 平行處理，使用OptimizeRead和OptimizeWrite
| 筆數(萬) | 中位數平均讀取(s) | 中位數平均寫入(s) | 總執行時間(s) |  記憶體用量(GB) |
|---------|------------------|------------------|--------------|----------------|
| 1,000   | 7.73             | 1.23             | 9.42         | 3.8            |
| 6,000   | 50.63            | 6.13             | 83.01        | 6.9            |
| 7,000   | 54.86            | 3.02             | 94.21        | 7.9            |
| 8,000   | 66.78            | 3.02             | 111.88       | 5.4            |
| 9,000   | 75.78            | 4.91             | 131.28       | 5.8            |
| 10,000  | 95.78            | 3.05             | 204.53       | 4.2            |
HW 製作 8000/9000/ 1E 資料
一路從1000萬 往上測下去 (以350萬為基準)