namespace BeachMobile.BillableSubscription.DataGateway.Redis

open Newtonsoft.Json
open StackExchange.Redis
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.Language

module Get =

    let status : GetRegistrationStatus =

        fun v -> async { 

            try
                let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
                let cache       = connection.GetDatabase()
                let response    = cache.StringGet(KeyFor.registration v.id)
            
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

    let paymentHistory : GetPaymentHistory = 

        fun v -> async { 

            let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
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