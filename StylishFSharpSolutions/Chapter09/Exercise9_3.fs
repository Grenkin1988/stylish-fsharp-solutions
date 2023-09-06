namespace StylishFSharpSolutions.Chapter09

open NUnit.Framework

module Exercise9_3 =
    let featureScale a b xMin xMax x =
        a + ((x - xMin) * (b - a)) / (xMax - xMin)

    let scale (data : seq<float>) =
        let minX = data |> Seq.min
        let maxX = data |> Seq.max
        let zeroOneScale = featureScale 0. 1. minX maxX
        data
        //|> Seq.map (fun x -> featureScale 0. 1. minX maxX x)
        |> Seq.map zeroOneScale

    [<TestFixture>]
    module Test =
        open FsUnit

        [<Test>]
        let ``Sequanse scaled to 0 - 1`` () =
            let result = seq [100.; 150.;200.] |> scale
            result |> should equal (seq { 0.0; 0.5; 1.0 })

