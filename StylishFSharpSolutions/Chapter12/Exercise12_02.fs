namespace StylishFSharpSolutions.Chapter12

module Exercise12_03 =

    open System
    open System.Text

    let private buildLine (data : float[]) =
        let cols = data |> Array.Parallel.map (sprintf "%f")
        // This call has been changed from taking a character separator
        // to a string separator - i.e. "," instead of ','. This is because
        // the character overload is not always available in scripts.
        String.Join(",", cols)

    let buildCsv (data : float[,]) =
        let sb = StringBuilder()
        for r in 0..(data |> Array2D.length1) - 1 do
            let row = data.[r, *]
            let rowString = row |> buildLine
            sb.AppendLine(rowString) |> ignore
        sb.ToString()

    let buildCsvFast (data : float[,]) =
        let sb = StringBuilder()
        let data = Array2D.map (sprintf "%f") data
        for r in 0..(data |> Array2D.length1) - 1 do
            let row = data.[r, *]
            let rowString = String.Join(",", row)
            sb.AppendLine(rowString) |> ignore
        sb.ToString()
