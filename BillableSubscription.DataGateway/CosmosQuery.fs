namespace BeachMobile.BillableSubscription.DataGateway.Cosmos

open Microsoft.Azure.Cosmos
open BeachMobile.BillableSubscription.Language
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.Entities
open BeachMobile.BillableSubscription.DataGateway.Common
open BeachMobile.BillableSubscription.DataGateway.Cosmos
open BeachMobile.BillableSubscription.DataGateway.Cosmos.Database
open BeachMobile.BillableSubscription.DataGateway.Redis

module Get =

    let status : GetRegistrationStatus =

        fun v -> task { 

            try
                let container = Container.get Database.name Container.registration

                match! container.ReadItemAsync<RegistrationStatusEntity>(v.id, PartitionKey(KeyFor.registrationStatus(v.Request.TenantId, v.Request.Plan))) |> Async.AwaitTask with
                | response when response.StatusCode = System.Net.HttpStatusCode.OK -> 
                    return Ok (Some response.Resource.Status)
                | _ -> return Error "Failed to retrieve registration status"

            with ex -> return ex |> toError
        }

    let paymentHistory : GetPaymentHistory = 

        fun v -> task {

            try
                let container = Container.get Database.name Container.paymentHistory

                match! container.ReadItemAsync<PaymentHistoryEntity>(v, PartitionKey(Container.paymentHistory)) |> Async.AwaitTask with
                | response when response.StatusCode = System.Net.HttpStatusCode.OK -> 
                    return Ok (Some response.Resource.Payments)
                | _ -> return Error "Failed to retrieve payment history"

            with ex -> return ex |> toError
        }