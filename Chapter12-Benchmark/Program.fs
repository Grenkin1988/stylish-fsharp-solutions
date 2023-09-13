open System
open BenchmarkDotNet.Running
open BenchmarkDotNet.Attributes

module Harness =
    [<MemoryDiagnoser>]
    type Harness() =
        [<Benchmark>]
        member _.Old() =
            SystemUnderTest.slowFunction()

        [<Benchmark>]
        member _.New() =
            SystemUnderTest.fastFunction()


[<EntryPoint>]
let main _ =
    BenchmarkRunner.Run<Harness.Harness>()
    |> printfn "%A"

    Console.ReadKey() |> ignore
    0
