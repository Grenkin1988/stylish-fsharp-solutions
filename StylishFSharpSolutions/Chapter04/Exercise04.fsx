#r "..\\bin\\Debug\\net7.0\\StylishFSharpSolutions.dll"

module Exercise04 =

    open StylishFSharpSolutions.Chapter04.Houses

    let houses = getHouses 20

    module Exercise04_01 = 
        let housePrices =
            houses
            |> Array.map (fun h -> sprintf "Address: %s - Price: %f" h.Address h.Price)

    module Exercise04_02 = 
        let averagePrice =
            houses
            |> Array.averageBy (fun h -> h.Price)

    module Exercise04_03 = 
        let expensiveHouses =
            houses |> Array.filter (fun h -> h.Price > 250_000M)

    module Exercise04_04 = 
        let houseToSchool =
            houses
            |> Array.choose (fun house ->
                trySchoolDistance house
                |> Option.bind (fun d -> Some(house, d)))

    module Exercise04_05 = 
        let printHouses() =
            houses 
            |> Array.filter (fun h -> h.Price > 100_000M)
            |> Array.iter (fun h -> printfn "House by address: %s costs: %f" h.Address h.Price)

    module Exercise04_06 = 
        let printHousesOrdered() =
            houses 
            |> Array.filter (fun h -> h.Price > 100_000M)
            |> Array.sortByDescending (fun h -> h.Price)
            |> Array.iter (fun h -> printfn "House by address: %s costs: %f" h.Address h.Price)

    module Exercise04_07 = 
        let averageOver200K =
            houses
            |> Array.filter (fun h -> h.Price > 200_000M)
            |> Array.averageBy (fun h -> h.Price)

    module Exercise04_08 = 
        let cheapHouseWithKnownSchoolDistance =
            houses
            |> Array.choose (fun house ->
                trySchoolDistance house
                |> Option.bind (fun d -> Some(house, d)))
            |> Array.find(fun (h,_) -> h.Price < 100_000M )

    module Exercise04_09 = 
        let housesByBand =
            houses
            |> Array.groupBy (fun h -> priceBand h.Price)
            |> Array.map (fun (band, houses) -> (band, Array.sortBy (fun h -> h.Price) houses ))

    module Exercise04_10 = 
        let inline tryAverageBy projection array =
            match array with
            | [||] -> None
            | array -> Array.averageBy projection array |> Some

        let avarage =
            houses
            |> Array.filter (fun h -> h.Price > 200_000M)
            |> tryAverageBy (fun h -> h.Price)

    module Exercise04_11 = 
        let cheapHouseWithKnownSchoolDistance =
            houses
            |> Array.choose (fun house ->
                trySchoolDistance house
                |> Option.bind (fun d -> Some(house, d)))
            |> Array.tryFind(fun (h,_) -> h.Price < 100_000M )
