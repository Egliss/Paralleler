# Paralleler
## Summary
Paralleler is additional implement of System.Threading.Parallel.
this library provide some usuful feature of the parallel system

+ `Parallel.For` with ordered index
+ AwaitableParallel 

## Others
### global.json
the file disable dotnet sdk of prerelease.
2020/09/05 dotnet sdk 5.0 preview 8 emit the error

```bash
Could not load file or assembly 'System.Runtime, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
```
