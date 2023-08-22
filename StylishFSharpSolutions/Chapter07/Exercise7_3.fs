module StylishFSharpSolutions.Chapter07.Exercise7_3

open NUnit.Framework
open System

type Track = {
    Name : string
    Artist : string
}

[<TestFixture>]
module Test =
    open FsUnit

    [<Test>]
    let structuralEqualityForRecords () =
        let tracks = 
            [   { Name = "The Mollusk"; Artist = "Ween" }
                { Name = "Bread Hair"; Artist = "They Might Be Giants" }
                { Name = "The Mollusk"; Artist = "Ween" }]
            |> Set.ofList
        tracks |> should haveCount 2
