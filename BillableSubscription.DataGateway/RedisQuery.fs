namespace BeachMobile.BillableSubscription.DataGateway.Redis

open System
open Newtonsoft.Json
open StackExchange.Redis
open Configuration
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.Language

module Get =

    let status : GetRegistrationStatus =

        fun v -> async { 

            let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
            let cache       = connection.GetDatabase()
            let response    = cache.StringGet(v.id)
            
            match response.HasValue with
            | false -> 
                do! connection.CloseAsync() |> Async.AwaitTask
                return Ok None

            | true  ->
                do! connection.CloseAsync() |> Async.AwaitTask
                return Ok (response |> JsonConvert.DeserializeObject<RegistrationStatus> |> Some)
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