module StylishFSharpSolutions.Chapter06.Exercise6_1

open NUnit.Framework
open System

type MeterValue =
    | Standard of int
    | Economy7 of Day:int * Night:int

type MeterReading =
    {
        ReadingDate : DateTime
        MeterValue : MeterValue
    }

let printDate (date : DateTime) = date.ToString("dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)

let formatReading (reading) =
    match reading with
    | { ReadingDate = date; MeterValue = Standard reading } ->
        sprintf "Your reading on: %s was %07i" (printDate date) reading
    | { ReadingDate = date; MeterValue =  Economy7(Day=day; Night=night) } ->
        sprintf "Your readings on: %s: Day: %07i Night: %07i" (printDate date) day night

let reading1 = { ReadingDate = new DateTime(2019, 3, 23); MeterValue = Standard 12982 }
let reading2 = { ReadingDate = new DateTime(2019, 2, 24); MeterValue = Economy7(Day=3432, Night=98218) }



[<TestFixture>]
module Test =
    open FsUnit

    [<Test>]
    let formatReading_reading1Test () =
        let actual = reading1 |> formatReading
        let expected = "Your reading on: 23/03/2019 was 0012982"
        actual |> should equal expected

    [<Test>]
    let formatReading_reading2Test () =
        let actual = reading2 |> formatReading
        let expected = "Your readings on: 24/02/2019: Day: 0003432 Night: 0098218"
        actual |> should equal expected
