namespace BeachMobile.BillableSubscription.DataGateway.Redis

open System
open Newtonsoft.Json
open StackExchange.Redis
open BeachMobile.BillableSubscription.Language
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.Entities

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

    let registration (v:RegistrationStatus) (connection:ConnectionMultiplexer) =

        task {
        
            let cache = connection.GetDatabase()
                
            let json    = JsonConvert.SerializeObject(v)
            let request = v.Registration.Request
            let key     = KeyFor.registrationStatus(request.TenantId, request.Plan)

            match register cache key json with
            | Error msg -> 
                do! connection.CloseAsync()
                return Error msg

            | Ok () ->

                do! connection.CloseAsync()
                    
                return Ok { id = Guid.NewGuid() |> string
                            Request   = request
                            Timestamp = DateTime.Now
                          }
            }

    let payment : SubmitPayment<ConnectionMultiplexer> = 
    
        fun v connection ->

            task {
        
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

    let paymentHistory (v:PaymentHistory) (connection:ConnectionMultiplexer) = 
    
        task {
        
            let cache = connection.GetDatabase()
            let json  = JsonConvert.SerializeObject(v)
                
            match register cache (KeyFor.paymentHistory v.SubscriptionId) json with
            | Error msg -> 
                do! connection.CloseAsync()
                return Error msg

            | Ok () ->
                do! connection.CloseAsync()
                return Ok()
        }