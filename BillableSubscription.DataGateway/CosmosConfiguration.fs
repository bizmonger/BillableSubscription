namespace BeachMobile.BillableSubscription.DataGateway.Cosmos

open Microsoft.Azure.Cosmos
open Azure.Identity

type ConnectionString() = static member val Instance = "" with get,set

module Container =

    let registration       = "Registration"
    let registrationStatus = "RegistrationStatus"
    let payments           = "Payments"
    let paymentHistory     = "PaymentHistory"

module Database =

    let name = "beachmobile-db"

    module Container =

        let get (db:string) (containerId:string) =

            let client    = new CosmosClient(ConnectionString.Instance, DefaultAzureCredential())
            let database  = client.GetDatabase(db)
            let container = database.GetContainer(containerId)
            container