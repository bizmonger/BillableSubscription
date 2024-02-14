namespace BeachMobile.BillableSubscription.DataGateway.Cosmos

open Microsoft.Azure.Cosmos

module Container =

    let registration       = "Registration"
    let registrationStatus = "RegistrationStatus"
    let payments           = "Payments"
    let paymentHistory     = "PaymentHistory"

module Database =

    let name = "beachmobile-db"

    module Container =

        let get (db:string) (containerId:string) (client:CosmosClient) =

            let database  = client.GetDatabase(db)
            let container = database.GetContainer(containerId)
            container