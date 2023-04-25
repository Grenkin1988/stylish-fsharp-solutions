namespace StylishFSharpSolutions.Chapter2.Exercise2_1

open NUnit.Framework
open System

module MilesYards =
    type MilesYards = private MilesYards of wholeMiles : int * yards : int

    let fromMilesPointYards (milesPointYards : float) : MilesYards =
        if milesPointYards < 0 then
            raise <| ArgumentOutOfRangeException("milesPointYards", "Distance must be >= 0")
        let wholeMiles = milesPointYards |> floor |> int
        let fraction = milesPointYards - float(wholeMiles)
        if fraction > 0.1759 then
            raise <| ArgumentOutOfRangeException("milesPointYards", "Fraction part must be <= 0.1759")
        let yards = fraction * 10_000. |> round |> int
        MilesYards(wholeMiles, yards)

    let toDecimalMiles (MilesYards(wholeMiles, yards)) : float =
        (float wholeMiles) + ((float yards) / 1760.)

[<TestFixture>]
module Test =
    [<Test>]
    let ConversionTest () =
        let milesPointYards = 1.0880
        let expected = 1.5
        let milesYards = MilesYards.fromMilesPointYards milesPointYards
        let actual = MilesYards.toDecimalMiles milesYards
        Assert.AreEqual(expected, actual)

    [<Test>]
    let YardsMustBeLessThen1760 () =
        let milesPointYards = 1.1760
        let result = Assert.Throws<ArgumentOutOfRangeException>(fun _ -> MilesYards.fromMilesPointYards milesPointYards |> ignore)
        StringAssert.StartsWith("Fraction part must be <= 0.1759", result.Message)

    [<Test>]
    let HandlingNegativeDistanceTest () =
        let milesPointYards = -0.01
        let result = Assert.Throws<ArgumentOutOfRangeException>(fun _ -> MilesYards.fromMilesPointYards milesPointYards |> ignore)
        StringAssert.StartsWith("Distance must be >= 0", result.Message)