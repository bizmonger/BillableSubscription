namespace BeachMobile.BillableSubscription.DataGateway.Redis

open System
open Newtonsoft.Json
open BeachMobile.BillableSubscription.Language
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.Entities
open StackExchange.Redis
open Configuration

// Documentation:
// https://medium.com/@sadigrzazada20/getting-started-with-redis-in-c-using-stackexchange-redis-353a9d65a136
// https://stackoverflow.com/questions/60927540/add-expiry-to-redis-cache

module Msg =

    let failedCacheItemRegistration = "Failed to register cache"
    let failedSetexpiration         = "Failed to set cache key expiration"

module Post =

    let set(cache:IDatabase) (key:string) (value:string) =

        match cache.StringSet(key, value) with
        | false -> Error Msg.failedCacheItemRegistration
        | true  -> 

            match cache.KeyExpire(key, new TimeSpan(0,0,30)) with
            | false -> Error Msg.failedSetexpiration
            | true  -> Ok ()

    let registration : RequestRegistration =

        fun v ->
            async {
        
                let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
                let cache = connection.GetDatabase()

                let data : RegistrationRequestEntity = {
                    id = Guid.NewGuid() |> string
                    PartitionKey = "Registration"
                    RegistrationRequest = v
                }
                
                let json = JsonConvert.SerializeObject(data)
                
                match set cache data.id json with
                | Error msg -> 
                    do! connection.CloseAsync() |> Async.AwaitTask
                    return Error msg

                | Ok () ->

                    do! connection.CloseAsync() |> Async.AwaitTask

                    return Ok {
                        id = Guid.NewGuid() |> string
                        Request   = v
                        Timestamp = DateTime.UtcNow
                    }                
            }

    let Payment : SubmitPayment = 
    
        fun v ->
            async {
        
                let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
                let cache = connection.GetDatabase()

                let data : PaymentRequestEntity = {
                    id = Guid.NewGuid() |> string
                    PartitionKey = "Payments"
                    PaymentRequest = v
                }

                let json = JsonConvert.SerializeObject(data)
                
                match set cache data.id json with
                | Error msg -> 
                    do! connection.CloseAsync() |> Async.AwaitTask
                    return Error msg

                | Ok () ->
                    do! connection.CloseAsync() |> Async.AwaitTask
                    return Ok { Payment= v; Timestamp= DateTime.UtcNow }
            }