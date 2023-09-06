module StylishFSharpSolutions.Chapter08.Exercise8_3

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

    override this.ToString() =
        sprintf "Greyscale(%i)" this.Level


[<TestFixture>]
module Test =
    open FsUnit

    [<Test>]
    let ``Black should give 0 in ToString`` () =
        let sut = GreyScale(Color.Black)
        sut
        |> sprintf "%O"
        |> should equal "Greyscale(0)"

    [<Test>]
    let ``White should give 255 in ToString`` () =
        let sut = GreyScale(Color.White)
        sut
        |> sprintf "%O"
        |> should equal "Greyscale(255)"

    [<Test>]
    let ``Brown should give 83 in ToString`` () =
        let sut = GreyScale(Color.Brown)
        sut
        |> sprintf "%O"
        |> should equal "Greyscale(83)"
