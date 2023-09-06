namespace StylishFSharpSolutions.Chapter09

open NUnit.Framework

module Exercise9_1 =
    let add a b = a + b

    let multiply a b = a * b

    let applyAndPrint f a b =
        let r = f a b
        sprintf "%i" r

    [<TestFixture>]
    module Test =
        open FsUnit

        [<Test>]
        let ``2 + 3 = 5`` () =
            let result = applyAndPrint add 2 3
            result |> should equal "5"

        [<Test>]
        let ``2 * 3 = 6`` () =
            let result = applyAndPrint multiply 2 3
            result |> should equal "6"

        [<Test>]
        let ``2 - 3 = -1`` () =
            let result = applyAndPrint (fun a b -> b |> multiply -1 |> add a) 2 3
            result |> should equal "-1"
