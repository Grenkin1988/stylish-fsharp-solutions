namespace StylishFSharpSolutions.Chapter03

module Exercise3_2 =
    type Delivery =
        | AsBilling
        | Physical of string
        | Download
        | ClickAndCollect of StoreId : int

    type BillingDetails = {
        name : string
        billing :  string
        delivery : Delivery }

    let countBillable (billingDetails : BillingDetails seq) =
        billingDetails
        |> Seq.sumBy (fun det -> 
            if System.String.IsNullOrWhiteSpace det.billing then
                0
            else 1)

    open NUnit.Framework

    [<TestFixture>]
    module Test =
        [<Test>]
        let NonNullCountBillableTest () =
            let details = [
                { name = "Kit Eason"; billing = "112 Fibonacci Street\nErehwon\n35813"; delivery = AsBilling };
                { name = "John Doe"; billing = "314 Pi Avenue\nErewhon\n15926"; delivery = Physical "16 Planck Parkway\nErewhon\n62291" };
                { name = "Jane Smith"; billing = "9 Gravity Road\nErewhon\n80665"; delivery = Download }
            ]

            let actual = countBillable details
            Assert.AreEqual(3, actual)

        [<Test>]
        let WithNullCountBillableTest () =
            let details = [
                { name = "Kit Eason"; billing = "112 Fibonacci Street\nErehwon\n35813"; delivery = AsBilling };
                { name = "John Doe"; billing = "314 Pi Avenue\nErewhon\n15926"; delivery = Physical "16 Planck Parkway\nErewhon\n62291" };
                { name = "Jane Smith"; billing = null; delivery = Download }
            ]

            let actual = countBillable details
            Assert.AreEqual(2, actual)
