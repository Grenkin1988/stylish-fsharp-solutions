#r "..\\bin\\Debug\\net7.0\\StylishFSharpSolutions.dll"

module Exercise04 =

    open StylishFSharpSolutions.Chapter04.Houses

    let houses = getHouses 20

    let housePrices =
        houses
        |> Array.map (fun h -> sprintf "Address: %s - Price: %f" h.Address h.Price)

    let averagePrice =
        houses
        |> Array.averageBy (fun h -> h.Price)

    let expensiveHouses =
        houses |> Array.filter (fun h -> h.Price > 250_000M)

    let houseToSchool =
        houses
        |> Array.choose (fun house ->
            trySchoolDistance house
            |> Option.bind (fun d -> Some(house, d)))
