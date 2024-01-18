namespace BeachMobile.BillableSubscription.DataGateway.Sync

open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.DataGateway

module Query =

    let status : GetRegistrationStatus =

        fun v -> async {
            
            match! Redis.Get.status v with
            | Error msg   -> return Error msg
            | Ok (Some r) -> return Ok (Some r)
            | Ok None -> 

                match! Redis.Post.registration v with
                | Error msg -> return Error msg
                | Ok r      -> return Ok (Some r)
        }

    let paymentHistory : GetPaymentHistory = 

        fun v -> async { 
        
            match! Redis.Get.paymentHistory v with
            | Error msg   -> return Error msg
            | Ok (Some r) -> return Ok (Some r)
            | Ok None -> 

                match! Cosmos.Get.paymentHistory v with
                | Error msg -> return Error msg
                | Ok None   -> 
                
                    match! Redis.Post.paymentHistory { SubscriptionId=v; Payments= Seq.empty} with
                    | Error msg -> return Error msg
                    | Ok ()     -> return Ok None

                | Ok (Some r) -> 
                
                    match! Redis.Post.paymentHistory {SubscriptionId= v; Payments= r} with
                    | Error msg -> return Error msg
                    | Ok ()     -> return Ok (Some r)
        }