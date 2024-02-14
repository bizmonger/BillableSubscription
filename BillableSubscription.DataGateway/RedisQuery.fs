namespace BeachMobile.BillableSubscription.DataGateway.Redis

open Newtonsoft.Json
open StackExchange.Redis
open Microsoft.Azure.Cosmos
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.Language

type SyncConnection = {
    Multiplexer  : ConnectionMultiplexer
    CosmosClient : CosmosClient
}

module Get =

    let status : GetRegistrationStatus<ConnectionMultiplexer> =

        fun v connection -> task { 

            try
                let cache    = connection.GetDatabase()
                let receipt  = v.Request
                let response = cache.StringGet(KeyFor.registrationStatus(receipt.TenantId, receipt.Plan))
            
                match response.HasValue with
                | false -> 
                    try
                        do! connection.CloseAsync() |> Async.AwaitTask
                        return Ok None

                    with ex -> return Error (ex.GetBaseException().Message)

                | true  ->
                    do! connection.CloseAsync() |> Async.AwaitTask
                    return Ok (response |> JsonConvert.DeserializeObject<RegistrationStatus> |> Some)

            with ex ->
                let msg = ex.GetBaseException().Message
                return Error msg
        }

    let paymentHistory : GetPaymentHistory<ConnectionMultiplexer> = 

        fun v connection -> task { 

            let cache    = connection.GetDatabase()
            let response = cache.StringGet(v)
            
            match response.HasValue with
            | false ->
                do! connection.CloseAsync() |> Async.AwaitTask
                return Ok None

            | true  ->
                do! connection.CloseAsync() |> Async.AwaitTask
                return Ok (response |> JsonConvert.DeserializeObject<SuccessfulPayment seq> |> Some)
        }