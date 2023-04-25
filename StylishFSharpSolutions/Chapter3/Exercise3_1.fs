namespace StylishFSharpSolutions.Chapter3

module Exercise3_1 =
    type Delivery =
        | AsBilling
        | Physical of string
        | Download
        | ClickAndCollect of StoreId : int

    type BillingDetails = {
        name : string
        billing :  string
        delivery : Delivery }

    let tryDeliveryLabel (billingDetails : BillingDetails) =
        match billingDetails.delivery with
        | AsBilling -> 
            billingDetails.billing |> Some
        | Physical address -> 
            address |> Some
        | Download -> None
        | ClickAndCollect _ -> None
        |> Option.map (fun address -> 
            sprintf "%s\n%s" billingDetails.name address)

    let tryCollectionsFor (stroreId : int) (billingDetails : BillingDetails) =
        match billingDetails.delivery with
        | ClickAndCollect store when stroreId = store -> Some billingDetails
        | AsBilling 
        | Physical _
        | Download
        | ClickAndCollect _ -> None

    let deliveryLabels (billingDetails : BillingDetails seq) =
        billingDetails
        |> Seq.choose tryDeliveryLabel

    let collectionsFor (stroreId : int) (billingDetails : BillingDetails seq) =
        billingDetails
        |> Seq.choose (tryCollectionsFor stroreId)

    open NUnit.Framework

    [<TestFixture>]
    module Test =
        [<Test>]
        let DeliveryLabelsTest () =
            let details = [
                { name = "Kit Eason"; billing = "112 Fibonacci Street\nErehwon\n35813"; delivery = AsBilling };
                { name = "John Doe"; billing = "314 Pi Avenue\nErewhon\n15926"; delivery = Physical "16 Planck Parkway\nErewhon\n62291" };
                { name = "Jane Smith"; billing = "9 Gravity Road\nErewhon\n80665"; delivery = Download }
            ]

            let expected = [
                "Kit Eason\n112 Fibonacci Street\nErehwon\n35813";
                "John Doe\n16 Planck Parkway\nErewhon\n62291"
            ]

            let actual = deliveryLabels details
            CollectionAssert.AreEqual(expected, actual)

        [<Test>]
        let NoClickAndCollectOrderCollectionsForTest () =
            let details = [
                { name = "Kit Eason"; billing = "112 Fibonacci Street\nErehwon\n35813"; delivery = AsBilling };
                { name = "John Doe"; billing = "314 Pi Avenue\nErewhon\n15926"; delivery = Physical "16 Planck Parkway\nErewhon\n62291" };
                { name = "Jane Smith"; billing = "9 Gravity Road\nErewhon\n80665"; delivery = Download }
            ]
            let storeId = 1234

            let expected = []

            let actual = collectionsFor storeId details
            CollectionAssert.AreEqual(expected, actual)

        [<Test>]
        let ClickAndCollectOrderCollectionsForTest () =
            let storeId = 1234
            let details = [
                { name = "Kit Eason"; billing = "112 Fibonacci Street\nErehwon\n35813"; delivery = AsBilling };
                { name = "John Doe"; billing = "314 Pi Avenue\nErewhon\n15926"; delivery = Physical "16 Planck Parkway\nErewhon\n62291" };
                { name = "Jane Smith"; billing = "9 Gravity Road\nErewhon\n80665"; delivery = Download };
                { name = "James Smith"; billing = "9 Gravity Road\nErewhon\n80665"; delivery = ClickAndCollect storeId };
                { name = "Oliver Boom"; billing = "14 Gravity Road\nErewhon\n80665"; delivery = ClickAndCollect 2345 }
            ]

            let expected = [details[3]]

            let actual = collectionsFor storeId details
            CollectionAssert.AreEqual(expected, actual)
