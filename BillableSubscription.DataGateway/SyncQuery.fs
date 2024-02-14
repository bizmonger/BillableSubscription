namespace BeachMobile.BillableSubscription.DataGateway.SyncLogic

open BeachMobile.BillableSubscription.Language
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.DataGateway
open BeachMobile.BillableSubscription.DataGateway.Redis

module Query =

    let status : GetRegistrationStatus<SyncConnection> =

        fun v connection -> task {

            let cache(status:RegistrationStatus) =

                async {
                
                    match! connection.Multiplexer |> Redis.Post.registration status |> Async.AwaitTask with
                    | Error msg  -> return Error msg
                    | Ok _ -> return Ok (Some status.Registration)
                }
        
            match! connection.Multiplexer |> Redis.Get.status v with
            | Error msg   -> return Error msg
            | Ok (Some r) -> return Ok (Some r)
            | Ok None -> 

                match! connection.CosmosClient |> Cosmos.Get.status v with
                | Error msg   -> return Error msg
                | Ok None     -> return Ok None
                | Ok (Some r) ->

                    match! cache r with
                    | Error msg -> return Error msg
                    | Ok _ -> return Ok (Some r)
        }

    let paymentHistory : GetPaymentHistory<SyncConnection> = 

        fun v connection -> task {

            let cache items =

                async {
                
                    match! connection.Multiplexer |> Redis.Post.paymentHistory { SubscriptionId=v; Payments= items} |> Async.AwaitTask with
                    | Error msg -> return Error msg
                    | Ok ()     -> return Ok None
                }
        
            match! connection.Multiplexer |> Redis.Get.paymentHistory v with
            | Error msg   -> return Error msg
            | Ok (Some r) -> return Ok (Some r)
            | Ok None -> 

                match! connection.CosmosClient |> Cosmos.Get.paymentHistory v with
                | Error msg   -> return Error msg
                | Ok None     -> return! cache Seq.empty
                | Ok (Some r) -> return! cache r
        }