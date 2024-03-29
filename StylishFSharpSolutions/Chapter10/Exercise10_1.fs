namespace StylishFSharpSolutions.Chapter10.Exercise10_1

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
    let GetData (count : int) =
        let strings = 
            Array.init count (fun i -> 
                Server.AsyncGetString i 
                |> Async.RunSynchronously)
        strings
        |> Array.sort

    let AsyncGetData (count : int) =
        async {
            let! strings = 
                Seq.init count (fun i -> Server.AsyncGetString i)
                |> Async.Parallel
            return (strings |> Array.sort)
        }

[<TestFixture>]
module Test =
    [<Test>]
    let ``Demo`` () =
        let sw = System.Diagnostics.Stopwatch()
        sw.Start()

        Consumer.GetData 10
        |> Array.iter (printfn "%s")
        printfn "That took %ims" sw.ElapsedMilliseconds

        Assert.Pass()

    [<Test>]
    let ``Demo async`` () =
        let sw = System.Diagnostics.Stopwatch()
        sw.Start()
 
        Consumer.AsyncGetData 10 
        |> Async.RunSynchronously
        |> Array.iter (printfn "%s")
        printfn "That took %ims" sw.ElapsedMilliseconds

        Assert.Pass()
