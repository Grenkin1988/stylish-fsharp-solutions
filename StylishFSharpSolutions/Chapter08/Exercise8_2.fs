module StylishFSharpSolutions.Chapter08.Exercise8_2

open NUnit.Framework
open System.Drawing

type GreyScale(r: byte, g: byte, b: byte) =
    let level =
        let average = (int r + int g + int b) / 3
        byte average

    new(color: Color) =
        GreyScale(color.R, color.G, color.B)

    member _.Level =
        level

[<TestFixture>]
module Test =
    open FsUnit

    [<Test>]
    let ``Black should give zero level`` () =
        let sut = GreyScale(Color.Black)
        sut.Level |> should equal 0uy

    [<Test>]
    let ``White should give 255 level`` () =
        let sut = GreyScale(Color.White)
        sut.Level |> should equal 255uy

    [<Test>]
    let ``Brown should give 83 level`` () =
        let sut = GreyScale(Color.Brown)
        sut.Level |> should equal 83uy
