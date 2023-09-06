namespace StylishFSharpSolutions.Chapter09

open NUnit.Framework

module Exercise9_4 =
    let applyAll (p : (float -> float) list) =
        p |> List.reduce (fun f1 f2 -> f1 >> f2)

    [<TestFixture>]
    module Test =
        open FsUnit

        let pipeline =
            [
                fun x -> x * 2.
                fun x -> x * x
                fun x -> x - 99.9
            ]

        [<Test>]
        let ``Apply all applies all functions`` () =
            let result = 100. |> applyAll pipeline
            result |> should equal 39900.1
