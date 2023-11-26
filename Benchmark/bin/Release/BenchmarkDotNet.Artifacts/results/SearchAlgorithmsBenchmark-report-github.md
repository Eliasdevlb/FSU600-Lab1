```

BenchmarkDotNet v0.13.10, Windows 10 (10.0.19045.3693/22H2/2022Update)
13th Gen Intel Core i5-13600KF, 1 CPU, 20 logical and 14 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9195.0), X86 LegacyJIT [AttachedDebugger]
  DefaultJob : .NET Framework 4.8.1 (4.8.9195.0), X86 LegacyJIT


```
| Method                  | N      | Mean           | Error         | StdDev        | Median         |
|------------------------ |------- |---------------:|--------------:|--------------:|---------------:|
| LinearSearchBestCase    | 100000 |      0.8694 ns |     0.0094 ns |     0.0079 ns |      0.8695 ns |
| LinearSearchAverageCase | 100000 | 24,889.8541 ns | 1,783.1138 ns | 5,201.4267 ns | 26,702.5658 ns |
| LinearSearchWorstCase   | 100000 | 51,944.3847 ns | 2,367.4054 ns | 6,792.5298 ns | 53,394.7571 ns |
| BinarySearchBestCase    | 100000 |     23.5355 ns |     0.4793 ns |     0.5129 ns |     23.6460 ns |
| BinarySearchAverageCase | 100000 |     23.5920 ns |     0.2053 ns |     0.1820 ns |     23.6413 ns |
| BinarySearchWorstCase   | 100000 |     23.0946 ns |     0.0544 ns |     0.0509 ns |     23.0633 ns |
| LambdaSearchBestCase    | 100000 |      6.6493 ns |     0.0403 ns |     0.0315 ns |      6.6324 ns |
| LambdaSearchAverageCase | 100000 | 78,378.9555 ns |   102.4701 ns |    95.8506 ns | 78,361.6455 ns |
| LambdaSearchWorstCase   | 100000 | 89,916.9582 ns | 1,759.1390 ns | 4,755.9347 ns | 90,943.9209 ns |
