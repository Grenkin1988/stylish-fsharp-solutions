namespace StylishFSharpSolutions.Chapter02.Exercise2_2

open NUnit.Framework
open System

module MilesChains =
    type MilesChains = private MilesChains of wholeMiles : int * chains : int

    let fromMilesChains (wholeMiles : int) (chains : int) : MilesChains =
        if wholeMiles < 0 then
            raise <| ArgumentOutOfRangeException("wholeMiles", "Miles must be >= 0")
        if chains < 0 || chains >= 80 then
            raise <| ArgumentOutOfRangeException("chains", "Chains must be >= 0 and < 80")
        MilesChains(wholeMiles, chains)

    let toDecimalMiles (MilesChains(wholeMiles, chains)) : float =
        (float wholeMiles) + ((float chains) / 80.)

[<TestFixture>]
module Test =
    [<Test>]
    let ConversionTest () =
        let miles = 1
        let chains = 40
        let expected = 1.5
        let milesYards = MilesChains.fromMilesChains miles chains
        let actual = MilesChains.toDecimalMiles milesYards
        Assert.AreEqual(expected, actual)

    [<Test>]
    let ChainsMustBeLessThen80 () =
        let result = Assert.Throws<ArgumentOutOfRangeException>(fun _ -> MilesChains.fromMilesChains 1 81 |> ignore)
        StringAssert.StartsWith("Chains must be >= 0 and < 80", result.Message)

    [<Test>]
    let HandlingNegativeMilesTest () =
        let milesPointYards = -0.01
        let result = Assert.Throws<ArgumentOutOfRangeException>(fun _ -> MilesChains.fromMilesChains -1 20 |> ignore)
        StringAssert.StartsWith("Miles must be >= 0", result.Message)

    [<Test>]
    let HandlingNegativeChainsTest () =
        let milesPointYards = -0.01
        let result = Assert.Throws<ArgumentOutOfRangeException>(fun _ -> MilesChains.fromMilesChains 2 -20 |> ignore)
        StringAssert.StartsWith("Chains must be >= 0 and < 80", result.Message)
