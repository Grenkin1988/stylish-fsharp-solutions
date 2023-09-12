namespace StylishFSharpSolutions.Chapter11

module Exercise11_01 =

    type Outcome<'TSuccess, 'TFailure> =
        | Success of 'TSuccess
        | Failure of 'TFailure

    let adapt func input =
        match input with
        | Success x -> func x
        | Failure f -> Failure f

    let passThrough func input =
        match input with
        | Success x -> func x |> Success
        | Failure f -> Failure f

    let passThroughRejects func input =
        match input with
        | Success x -> Success x
        | Failure f -> func f |> Failure

    open NUnit.Framework

    [<TestFixture>]
    module Test =
        open FsUnit

        [<Test>]
        let ``Pass Through Rejects`` () =
            let result = 2 |> Failure |> passThroughRejects (fun f -> f * 100)
            let expected = 200 |> Failure
            result |> should equal expected
