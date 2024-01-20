namespace BeachMobile.BillableSubscription.DataGateway.Cosmos

open Microsoft.Azure.Cosmos
open BeachMobile.BillableSubscription.Language
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.Entities
open BeachMobile.BillableSubscription.DataGateway.Cosmos
open BeachMobile.BillableSubscription.DataGateway.Cosmos.Database

module Get =

    let status : GetRegistrationStatus =

        fun v -> async { 

            try
                let container = Container.get Database.name Container.registration

                match! container.ReadItemAsync<RegistrationStatusEntity>(v.id, PartitionKey(KeyFor.)) |> Async.AwaitTask with
                | response when response.StatusCode = System.Net.HttpStatusCode.OK -> 
                    return Ok (Some response.Resource.Status)
                | _ -> return Error "Failed to retrieve registration status"

            with ex ->
                let msg = ex.GetBaseException().Message
                return Error msg
        }

    let paymentHistory : GetPaymentHistory = 

        fun v -> async {

            let container = Container.get Database.name Container.paymentHistory

            match! container.ReadItemAsync<PaymentHistoryEntity>(v, PartitionKey(Container.paymentHistory)) |> Async.AwaitTask with
            | response when response.StatusCode = System.Net.HttpStatusCode.OK -> 
                return Ok (Some response.Resource.Payments)
            | _ -> return Error "Failed to retrieve payment history"
        }