namespace StylishFSharpSolutions.Chapter13

module Exercise13_01 = 

    open System.IO
    open System.Text.RegularExpressions

    let find pattern dir =
        let re = Regex(pattern)
        Directory.EnumerateFiles
                      (dir, "*.*", SearchOption.AllDirectories)
        |> Seq.filter (fun path -> re.IsMatch(Path.GetFileName(path)))
        |> Seq.map (fun path -> 
            FileInfo(path))
        |> Seq.filter (fun fi -> 
                    fi.Attributes.HasFlag(FileAttributes.ReadOnly))
        |> Seq.map (fun fi -> fi.Name)

    let findBetter pattern dir =
        let re = Regex(pattern)
        Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories)
        |> Seq.filter (fun path -> 
            let fileName = Path.GetFileName(path)
            re.IsMatch(fileName))
        |> Seq.map (fun fileName -> FileInfo(fileName))
        |> Seq.filter (fun fi -> fi.Attributes.HasFlag(FileAttributes.ReadOnly))
        |> Seq.map (fun fi -> fi.Name)

module Exercise13_01_Better = 
    open System.IO
    open System.Text.RegularExpressions

    module FileSearch =
        module private FileName =
            let isMatchPattern (regex : Regex) =
                fun (path : string) ->
                    let fileName = Path.GetFileName(path)
                    regex.IsMatch(fileName)

            let isMatch pattern =
                let re = Regex(pattern)
                isMatchPattern re

        module private FileAttributes =
            let hasFlag flag fileName =
                FileInfo(fileName)
                    .Attributes
                    .HasFlag(flag)

        let findReadOnly pattern dir =
            let regex = Regex(pattern)
            Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories)
            |> Seq.filter (FileName.isMatchPattern regex)
            |> Seq.filter (FileAttributes.hasFlag FileAttributes.ReadOnly)
