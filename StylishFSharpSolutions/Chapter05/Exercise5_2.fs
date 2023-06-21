module StylishFSharpSolutions.Chapter05.Exercise5_2

open NUnit.Framework

let extremes (s : seq<float>) =
    let first = Seq.head s
    s
    |> Seq.fold (fun (minimum, maximum) next -> (min minimum next, max maximum next )) (first,first)

let extremes' (s : seq<float>) =
    s |> Seq.min,
    s |> Seq.max

[<TestFixture>]
module Test =
    open FsUnit

    [<Test>]
    let ExtremesTest () =
        let input = [| 1.0; 2.3; 11.1; -5. |]
        let expected = (-5., 11.1)
        let actual = extremes input
        actual |> should equal expected

    open System
    open System.Diagnostics

    [<Test>]
    let ExtremesPerfTest () =
        let r = Random()
        let big = Array.init 1_000_000 (fun _ -> r.NextDouble())
        let sw = Stopwatch()
        sw.Start()
        let min, max = extremes big
        sw.Stop()
        (min, max) |> should not' (be null)
        printf "min: %f  max: %f - time: %ims" min max sw.ElapsedMilliseconds

    [<Test>]
    let ExtremesPerfTest' () =
        let r = Random()
        let big = Array.init 1_000_000 (fun _ -> r.NextDouble())
        let sw = Stopwatch()
        sw.Start()
        let min, max = extremes' big
        sw.Stop()
        (min, max) |> should not' (be null)
        printf "min: %f  max: %f - time: %ims" min max sw.ElapsedMilliseconds
