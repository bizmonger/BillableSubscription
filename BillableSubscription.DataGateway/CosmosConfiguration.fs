namespace BeachMobile.BillableSubscription.DataGateway.Cosmos

open Microsoft.Azure.Cosmos
open Azure.Identity

type ConnectionString() = static member val Instance = "" with get,set

module Partition =

    let registration       = "Registration"
    let registrationStatus = "RegistrationStatus"
    let payments           = "Payments"
    let paymentHistory     = "PaymentHistory"

module Database =

    let name = "beachmobile-db"

    let container (db:string) (partitionKey:string) =

        let client    = new CosmosClient(ConnectionString.Instance, DefaultAzureCredential())
        let database  = client.GetDatabase(db)
        let container = database.GetContainer(partitionKey)
        container