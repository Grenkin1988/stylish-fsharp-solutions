namespace StylishFSharpSolutions.Chapter12

module Exercise12_01 =

    type Transaction = { Id : int } // Would contain more fields in reality

    let addTransactions
        (oldTransactions : Transaction list)
        (newTransactions : Transaction list) =
        oldTransactions @ newTransactions

    let addTransactionsFaster
        (oldTransactions : Transaction[])
        (newTransactions : Transaction[]) =
        Array.append oldTransactions newTransactions

