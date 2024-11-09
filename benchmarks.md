# Benchmarks

## cat

|OS   |File   |Test  |Built-in cat|C# cat|
|-----|-------|------|:------------:|:------:|
|linux|enwiki8|stdout| 1.43s        |1.65s   |
|     |       |      | 1.31s        |1.77s   |
|     |       |      | 1.27s        |1.67s   |
|     |enwiki9|stdout|13.42s        |16.27s  |
|     |       |      |13.29s        |16.36s  |
|     |       |      |13.34s        |16.45s  |
|linux|enwiki8|file  |0.008s        |1.26s   |
|     |       |      |0.002s        |1.26s   |
|     |       |      |0.003s        |1.26s   |
|     |enwiki9|file  |0.021s        |12.06s  |
|     |       |      |0.022s        |11.98s  |
|     |       |      |0.018s        |12.06s  |

### Methodology

`clear ; time ./cat ~/Downloads/enwik8`

`clear ; time ./cat ~/Downloads/enwik8`

`rm ./target8 ; time cat ~/Downloads/enwik8 > target8`

`rm ./target9 ; time cat ~/Downloads/enwik9 > target9`