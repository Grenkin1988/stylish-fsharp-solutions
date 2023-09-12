namespace StylishFSharpSolutions.Chapter11

module Exercise11_02 =

    open System

    type Message = 
        { 
            FileName : string
            Content : float[] 
        }

    type Reading =
        { 
            TimeStamp : DateTimeOffset
            Data : float[] 
        }

    let example =
        [|
            {   FileName = "2019-02-23T02:00:00-05:00"
                Content = [|1.0; 2.0; 3.0; 4.0|] }
            {   FileName = "2019-02-23T02:00:10-05:00"
                Content = [|5.0; 6.0; 7.0; 8.0|] }
            {   FileName = "error"
                Content = [||] }
            {   FileName = "2019-02-23T02:00:20-05:00"
                Content = [|1.0; 2.0; 3.0; Double.NaN|] }
        |]

    let log s = printfn "Logging: %s" s

    type MessageError =
        | InvalidFileName of fileName:string
        | DataContainsNaN of fileName:string * index:int

    let getReading message =
        let fileName = message.FileName
        match DateTimeOffset.TryParse(fileName) with
        | true, dt -> 
            let reading = { TimeStamp = dt; Data = message.Content }
            Ok (fileName, reading)
        | false, _ -> 
            fileName |> InvalidFileName |> Error

    let validateData(fileName, reading) =
        let nanIndex = 
            reading.Data
            |> Array.tryFindIndex (Double.IsNaN)
        match nanIndex with
        | Some i -> 
            (fileName,i) |> DataContainsNaN |> Error
        | None -> 
            Ok reading

    let logError (e : MessageError) =
        match e with
        | InvalidFileName fn ->
            log (sprintf "Invalid file name: %s" fn)
        | DataContainsNaN (fn, i) ->
            log (sprintf "Data contains NaN at position: %i in file: %s" i fn)

    open NUnit.Framework

    [<TestFixture>]
    module Test =
        open FsUnit

        [<Test>]
        let ``Demo`` () =
            let processMessage =
                getReading
                >> Result.bind validateData
                >> Result.mapError logError

            let processData data =
                data
                |> Array.map processMessage
                |> Array.choose (fun result -> 
                    match result with
                    | Ok reading -> reading |> Some
                    | Error _ -> None)
            
            let demo() =
                example
                |> processData
                |> Array.iter (printfn "%A")
            demo()

