# Paralleler
[![NuGet version (Paralleler)](https://img.shields.io/nuget/v/Paralleler.svg?style=flat-square)](https://www.nuget.org/packages/Paralleler/)
![.NET Core](https://github.com/Egliss/Paralleler/workflows/.NET%20Core/badge.svg?branch=master)
## Summary
Paralleler is additional implement of System.Threading.Parallel.  
this library provide some usuful feature of the parallel system.  

+ `Parallel.For` with ordered index
+ AwaitableParallel 

## System.Threading.Tasks.Parallel

`Parallel.For` is running function with unordered index.
for example,

```cs
using System.Threading.Tasks;
void Function()
{
    Parallel.For(0, 100, (int index) => Console.WriteLine(index));
}
```
the code will output next result.
```sh
60
54
55
...
0
30
24
61
62
```
If you can't accept this result, my library may be useful.

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
**index is not synchronized** so, encounting 95 after 97.
OrderedParallel.For() just tries to perform operations in ascending order as much as possible.
  
This library won't help if you need a tighter index sync.