﻿namespace BeachMobile.BillableSubscription.DataGateway.Redis

open System
open Newtonsoft.Json
open BeachMobile.BillableSubscription.Language
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.Entities
open BeachMobile.BillableSubscription.DataGateway
open BeachMobile.BillableSubscription.DataGateway.Cosmos
open StackExchange.Redis

// Documentation:
// https://medium.com/@sadigrzazada20/getting-started-with-redis-in-c-using-stackexchange-redis-353a9d65a136
// https://stackoverflow.com/questions/60927540/add-expiry-to-redis-cache

module Post =

    let private register(cache:IDatabase) (key:string) (value:string) =

        match cache.StringSet(key, value) with
        | false -> Error Msg.failedCacheItemRegistration
        | true  -> 

            match cache.KeyExpire(key, new TimeSpan(0,0,30)) with
            | false -> Error Msg.failedSetexpiration
            | true  -> Ok ()

    let registration (v:RegistrationStatus) =

        async {
        
            let! connection = ConnectionMultiplexer.ConnectAsync(Redis.ConnectionString.Instance) |> Async.AwaitTask
            let cache = connection.GetDatabase()
                
            let json    = JsonConvert.SerializeObject(v)
            let receipt = v.Registration.Request
            let key     = KeyFor.registrationStatus(receipt.TenantId, receipt.Plan)

            match register cache key json with
            | Error msg -> 
                do! connection.CloseAsync() |> Async.AwaitTask
                return Error msg

            | Ok () ->

                do! connection.CloseAsync() |> Async.AwaitTask
                    
                return Ok()
            }

    let payment : SubmitPayment = 
    
        fun v ->
            task {
        
                let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
                let cache = connection.GetDatabase()

                let data : PaymentRequestEntity = {
                    id = Guid.NewGuid() |> string
                    PartitionId = "hello-world"
                    PaymentRequest = v
                }

                let json = JsonConvert.SerializeObject(data)
                
                match register cache (KeyFor.payment data.id) json with
                | Error msg -> 
                    do! connection.CloseAsync() |> Async.AwaitTask
                    return Error msg

                | Ok () ->
                    do! connection.CloseAsync() |> Async.AwaitTask
                    return Ok { Payment= v; Timestamp= DateTime.UtcNow }
            }

    let paymentHistory (v:PaymentHistory) = 
    
        async {
        
            let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
            let cache = connection.GetDatabase()

            let json = JsonConvert.SerializeObject(v)
                
            match register cache (KeyFor.paymentHistory v.SubscriptionId) json with
            | Error msg -> 
                do! connection.CloseAsync() |> Async.AwaitTask
                return Error msg

            | Ok () ->
                do! connection.CloseAsync() |> Async.AwaitTask
                return Ok()
        }