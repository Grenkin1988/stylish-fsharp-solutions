module Log =
    open System
    open System.Threading

    let report =
        let lockObj = obj()
        fun color message ->
            lock lockObj (fun _ ->
                Console.ForegroundColor <- color
                printfn "%s (thread ID: %i)" message Thread.CurrentThread.ManagedThreadId
                Console.ResetColor())

    let red = report ConsoleColor.Red
    let green = report ConsoleColor.Green
    let yellow = report ConsoleColor.Yellow
    let cyan = report ConsoleColor.Cyan

module Outcome =
    type Outcome =
        | Ok of filename:string
        | Failed of filename:string

    let isOk = function
        | Ok _ -> true
        | Failed _ -> false

    let fileName = function
        | Ok fn
        | Failed fn -> fn

module Download =
    open System
    open System.IO
    open System.Net
    open System.Net.Http
    open System.Text.RegularExpressions
    open FSharp.Data
    open FSharpx.Control

    let private absoluteUri (pageUri : Uri) (filePath : string) =
        if filePath.StartsWith("http:") || filePath.StartsWith("https:") then
            Uri(filePath)
        else
            let sep = '/'
            filePath.TrimStart(sep)
            |> sprintf "%O%c%s" pageUri sep
            |> Uri

    let private getLinks (pageUri : Uri) (filePattern : string) =
        Log.cyan "Getting names..."
        let regex = Regex(filePattern)
        let html = HtmlDocument.Load(pageUri.AbsoluteUri)

        let links =
            html.Descendants ["a"]
            |> Seq.choose (fun node ->
                node.TryGetAttribute("href")
                |> Option.map (fun att -> att.Value()))
            |> Seq.filter (regex.IsMatch)
            |> Seq.map (absoluteUri pageUri)
            |> Seq.distinct
            |> Array.ofSeq

        links

    let private getLinksAsync (pageUri : Uri) (filePattern : string) =
        async {
            Log.cyan "Getting names..."
            let regex = Regex(filePattern)
            let! html = HtmlDocument.AsyncLoad(pageUri.AbsoluteUri)

            let links =
                html.Descendants ["a"]
                |> Seq.choose (fun node ->
                    node.TryGetAttribute("href")
                    |> Option.map (fun att -> att.Value()))
                |> Seq.filter (regex.IsMatch)
                |> Seq.map (absoluteUri pageUri)
                |> Seq.distinct
                |> Array.ofSeq

            return links
        }

    let private downloadFile (fileUri : Uri) (filePath : string) =
        use client = new HttpClient()
        let data = client.GetByteArrayAsync(fileUri) |> Async.AwaitTask |> Async.RunSynchronously
        File.WriteAllBytes(filePath, data)

    let private tryDownload (localPath : string) (fileUri : Uri) =
        let fileName = fileUri.Segments |> Array.last
        Log.yellow (sprintf "%s - starting download" fileName)
        let filePath = Path.Combine(localPath, fileName)

        try
            downloadFile fileUri filePath
            Log.green (sprintf "%s - download complete" fileName)
            Outcome.Ok fileName
        with
        | e ->
            Log.red (sprintf "%s - error: %s" fileName e.Message)
            Outcome.Failed fileName

    let private asyncDownloadFile (fileUri : Uri) (filePath : string) =
        async {
            use client = new HttpClient()
            let! data = client.GetByteArrayAsync(fileUri) |> Async.AwaitTask
            return! File.WriteAllBytesAsync(filePath, data) |> Async.AwaitTask
        }

    let private tryDownloadAsync (localPath : string) (fileUri : Uri) =
        async {
            let fileName = fileUri.Segments |> Array.last
            Log.yellow (sprintf "%s - starting download" fileName)
            let filePath = Path.Combine(localPath, fileName)

            try
                do! asyncDownloadFile fileUri filePath
                Log.green (sprintf "%s - download complete" fileName)
                return (Outcome.Ok fileName)
            with
            | e ->
                Log.red (sprintf "%s - error: %s" fileName e.Message)
                return (Outcome.Failed fileName)
        }

    let GetFiles (pageUri : Uri) filePattern localPath =
        let links = getLinks pageUri filePattern
        let downloaded, failed =
            links
            |> Array.map (tryDownload localPath)
            |> Array.partition Outcome.isOk

        downloaded |> Array.map Outcome.fileName,
        failed |> Array.map Outcome.fileName

    let AsyncGetFiles (pageUri : Uri) filePattern localPath =
        async {
            let! links = getLinksAsync pageUri filePattern

            let! downloadResults =
                links
                |> Seq.map (tryDownloadAsync localPath)
                |> Async.Parallel

            let downloaded, failed =
                downloadResults
                |> Array.partition Outcome.isOk

            return
                downloaded |> Array.map Outcome.fileName,
                failed |> Array.map Outcome.fileName
        }

    let AsyncGetFilesBatched (pageUri : Uri) filePattern localPath batchSize =
        async {
            let! links = getLinksAsync pageUri filePattern

            let downloaded, failed =
                links
                |> Seq.map (tryDownloadAsync localPath)
                |> Seq.chunkBySize batchSize
                |> Seq.collect (fun batch -> 
                    batch
                    |> Async.Parallel
                    |> Async.RunSynchronously)
                |> Array.ofSeq
                |> Array.partition Outcome.isOk

            return
                downloaded |> Array.map Outcome.fileName,
                failed |> Array.map Outcome.fileName
        }

    let AsyncGetFilesThrottled (pageUri : Uri) filePattern localPath throttle =
        async {
            let! links = getLinksAsync pageUri filePattern

            let! downloadResults =
                links
                |> Seq.map (tryDownloadAsync localPath)
                |> Async.ParallelWithThrottle throttle

            let downloaded, failed =
                downloadResults
                |> Array.partition Outcome.isOk

            return
                downloaded |> Array.map Outcome.fileName,
                failed |> Array.map Outcome.fileName
        }

open System
open System.Diagnostics

[<EntryPoint>]
let main argv =
    let uri = Uri @"https://minorplanetcenter.net/data"
    let pattern = @"neam.*\.json\.gz$"

    let localPath = @"D:\Workspace\temp\downloads"

    let sw = Stopwatch()
    sw.Start()

    //let downloaded, failed = Download.GetFiles uri pattern localPath

    let downloaded, failed = 
        Download.AsyncGetFiles uri pattern localPath 
        |> Async.RunSynchronously

    failed |> Array.iter (fun fn -> Log.red (sprintf "Failed: %s" fn))

    sprintf "%i files downloaded in %0.1fs, %i failed. Press a key" downloaded.Length sw.Elapsed.TotalSeconds failed.Length
    |> Log.cyan

    Console.ReadKey() |> ignore
    0
