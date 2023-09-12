namespace StylishFSharpSolutions.Chapter10.Exercise10_2

open System
open NUnit.Framework

module Random =
    let private random = System.Random()

    let string() =
        let len = random.Next(0, 10)
        Array.init len (fun _ -> random.Next(0, 255) |> char)
        |> String

module Server =
    let AsyncGetString (id : int) = 
        // id is unused
        async {
            do! Async.Sleep(500)
            return Random.string()
        }

module Consumer =
    let GetDataAsync (count : int) =
        async {
            let! strings = 
                Seq.init count (fun i -> Server.AsyncGetString i)
                |> Async.Parallel
            return (strings |> Array.sort)
        } |> Async.StartAsTask

[<TestFixture>]
module Test =
    [<Test>]
    let ``Demo task`` () =
        let sw = System.Diagnostics.Stopwatch()
        sw.Start()

        Consumer.GetDataAsync 10
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> Array.iter (printfn "%s")
        printfn "That took %ims" sw.ElapsedMilliseconds

        Assert.Pass()
