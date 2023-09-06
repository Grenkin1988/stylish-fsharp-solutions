namespace StylishFSharpSolutions.Chapter09

open NUnit.Framework

module Exercise9_2 =
    let counter start =
        let mutable current = start
        fun () ->
            let this = current
            current <- current + 1
            this

    let rangeCounter start until =
        let mutable current = start
        fun () ->
            let this = current
            let next = current + 1
            if next <= until then
                current <- next
            else
                current <- start
            this

    [<TestFixture>]
    module Test =
        open FsUnit

        [<Test>]
        let ``Counter from 0`` () =
            let c = counter 0
            let result = [0 .. 4] |> List.map (fun _ -> c())
            result |> should equal [0;1;2;3;4]

        [<Test>]
        let ``Counter from 100`` () =
            let c = counter 100
            let result = [0 .. 4] |> List.map (fun _ -> c())
            result |> should equal [100;101;102;103;104]

        [<Test>]
        let ``Range counter from 0 until 10 for 5`` () =
            let c = rangeCounter 0 10
            let result = [0 .. 4] |> List.map (fun _ -> c())
            result |> should equal [0;1;2;3;4]

        [<Test>]
        let ``Range counter from 0 until 10 for 13`` () =
            let c = rangeCounter 0 10
            let result = [0 .. 12] |> List.map (fun _ -> c())
            result |> should equal [0;1;2;3;4;5;6;7;8;9;10;0;1]
