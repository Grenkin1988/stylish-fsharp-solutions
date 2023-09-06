module StylishFSharpSolutions.Chapter08.Exercise8_4

open NUnit.Framework
open System
open System.Drawing

type GreyScale(r: byte, g: byte, b: byte) =
    let level =
        let average = (int r + int g + int b) / 3
        byte average

    let eq (that: GreyScale) =
        level = that.Level

    new(color: Color) =
        GreyScale(color.R, color.G, color.B)

    member _.Level =
        level

    override this.ToString() =
        sprintf "Greyscale(%i)" this.Level

    override _.Equals(obj) =
        match obj with
        | :? GreyScale as that -> eq that
        | _ -> false

    override _.GetHashCode() = hash level

    interface IEquatable<GreyScale> with
        member _.Equals(that: GreyScale) = eq that

[<TestFixture>]
module Test =
    open FsUnit

    [<Test>]
    let ``Orange equals to (0xFFuy, 0xA5uy, 0x00uy)`` () =
        let sut = GreyScale(Color.Orange)
        let that = GreyScale(0xFFuy, 0xA5uy, 0x00uy)
        sut |> should equal that

    [<Test>]
    let ``Orange not equals to Blue`` () =
        let sut = GreyScale(Color.Orange)
        let that = GreyScale(Color.Blue)
        sut |> should not' (equal that)

    [<Test>]
    let ``(0xFFuy, 0xA5uy, 0x00uy) equals to (0xFFuy, 0xA5uy, 0x01uy)`` () =
        let sut = GreyScale(0xFFuy, 0xA5uy, 0x00uy)
        let that = GreyScale(0xFFuy, 0xA5uy, 0x01uy)
        // This is because average to get level would return same value
        sut |> should equal that
