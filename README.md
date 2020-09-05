# Paralleler
## Summary
Paralleler is additional implement of System.Threading.Parallel.  
this library provide some usuful feature of the parallel system.  

+ `Parallel.For` with ordered index
+ AwaitableParallel 

## Usage
```cs
using System;
using Egliss.Paralleler;

async Task Function()
{
    await OrderedParallel.ForAsync(0, 100, Environment.ProcessorCount / 2, (index) => Console.WriteLine(index));
}
```
output
```sh
0
1
2
...
97
98
99
```