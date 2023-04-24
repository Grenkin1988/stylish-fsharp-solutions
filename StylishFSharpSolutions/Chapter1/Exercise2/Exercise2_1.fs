namespace Chapter1.Exercise2.Exercise2_1

open NUnit.Framework

module MilesYards =
    open System

    type MilesYards = private MilesYards of wholeMiles : int * yards : int

    let fromMilesPointYards (milesPointYards: float) : MilesYards =
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