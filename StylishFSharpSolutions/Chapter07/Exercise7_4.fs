module StylishFSharpSolutions.Chapter07.Exercise7_4

open NUnit.Framework
open System

[<Struct>]
type Position = {
    X : float32
    Y : float32
    Z : float32
    Time : DateTime
}

module Position =
    let translate dx dy dz position =
        { position with X = position.X + dx; Y = position.Y + dy; Z = position.Z + dz }

[<TestFixture>]
module Test =
    open FsUnit

    [<Test>]
    let testTranslate () =
        let p1 =
            { X = 1.0f
              Y = 2.0f
              Z = 3.0f
              Time = DateTime.MinValue }
        let expected =
            { X = 1.5f
              Y = 1.5f
              Z = 4.5f
              Time = DateTime.MinValue }
        let result = Position.translate 0.5f -0.5f 1.5f p1
        result |> should equal expected
