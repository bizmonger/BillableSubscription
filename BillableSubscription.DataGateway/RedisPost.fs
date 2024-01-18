namespace BeachMobile.BillableSubscription.DataGateway.Redis

open System
open BeachMobile.BillableSubscription.Language
open BeachMobile.BillableSubscription.Operations
open StackExchange.Redis
open Configuration

// Documentation:
// https://medium.com/@sadigrzazada20/getting-started-with-redis-in-c-using-stackexchange-redis-353a9d65a136
// https://stackoverflow.com/questions/60927540/add-expiry-to-redis-cache

module Msg =

    let failedCacheItemRegistration = "Failed to register cache"

module Post =

    let registration : RequestRegistration =

        fun v ->
            async {
        
                let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
                let cache = connection.GetDatabase()
                
                match cache.StringSet("?", "?") with
                | false -> return Error Msg.failedCacheItemRegistration
                | true  -> 

                    match cache.KeyExpire("someKey", new TimeSpan(0,0,30)) with
                    | false -> return Error ""
                    | true  -> 
                           
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
                
                match cache.StringSet("?", "?") with
                | false -> return Error Msg.failedCacheItemRegistration
                | true  -> 

                    match cache.KeyExpire("someKey", new TimeSpan(0,0,30)) with
                    | false -> return Error ""
                    | true  ->
                           
                        return Ok {
                            Payment   = v
                            Timestamp = DateTime.UtcNow
                        }          
            }