module StylishFSharpSolutions.Chapter06.Exercise6_2

open NUnit.Framework
open System

type FruitBatch = { Name : string; Count : int }

let fruits =
    [ { Name="Apples"; Count=3 }
      { Name="Oranges"; Count=4 }
      { Name="Bananas"; Count=2 } ]

// There are 3 Apples
// There are 4 Oranges
// There are 2 Bananas
for { Name = name; Count = count } in fruits do
    printfn "There are %i %s" count name

// There are 3 Apples
// There are 4 Oranges
// There are 2 Bananas
fruits
|> List.iter (fun { Name = name; Count = count } ->
    printfn "There are %i %s" count name)
