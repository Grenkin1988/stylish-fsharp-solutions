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
