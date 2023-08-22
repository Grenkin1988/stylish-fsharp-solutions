open System

#r "..\\bin\\Debug\\net7.0\\StylishFSharpSolutions.dll"

[<Struct>]
type Position = {
    X : float32
    Y : float32
    Z : float32
    Time : DateTime
}

#time "on"

let test =
    Array.init 1_000_000 (fun i -> 
        {   X = float32 i
            Y = float32 i
            Z = float32 i
            Time = DateTime.MinValue })

#time "off"
