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
    await OrderedParallel.ForAsync(0, 100, (index) => Console.WriteLine(index), Environment.ProcessorCount / 2);
}
```
output
```sh
0
1
2
...
96
97
95 
98
99
```
**index is not synchronize**
but, OrderedParallel.For() will try to perform operations in ascending order as much as possible.
