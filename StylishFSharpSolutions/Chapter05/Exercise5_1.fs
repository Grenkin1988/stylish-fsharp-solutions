module StylishFSharpSolutions.Chapter05.Exercise5_1

open NUnit.Framework

let clip ceilling s =
    s
    |> Seq.map (fun v -> min ceilling v)

[<TestFixture>]
module Test =
    open FsUnit

    [<Test>]
    let ClipTest () =
        let input = [| 1.0; 2.3; 11.1; -5. |]
        let expected = seq { 1.0; 2.3; 10.0; -5.0 }
        let actual = clip 10. input
        actual |> should equalSeq expected
