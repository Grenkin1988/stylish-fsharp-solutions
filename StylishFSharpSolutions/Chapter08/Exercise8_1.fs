module StylishFSharpSolutions.Chapter08.Exercise8_1

open NUnit.Framework

type GreyScale(r: byte, g: byte, b: byte) =
    let level =
        let average = (int r + int g + int b) / 3
        byte average

    member _.Level =
        level

[<TestFixture>]
module Test =
    open FsUnit

    [<Test>]
    let ``Zero RGB should give zero level`` () =
        let sut = GreyScale(0uy,0uy,0uy)
        sut.Level |> should equal 0uy

    [<Test>]
    let ``255 RGB should give 255 level`` () =
        let sut = GreyScale(255uy,255uy,255uy)
        sut.Level |> should equal 255uy
