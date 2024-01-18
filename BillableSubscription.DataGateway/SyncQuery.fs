namespace BeachMobile.BillableSubscription.DataGateway.Sync

open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.DataGateway

module Query =

    let status : GetRegistrationStatus =

        fun v -> async {
            
            match! Redis.Get.status v with
            | Error msg   -> return Error msg
            | Ok None     -> return! Cosmos.Get.status v
            | Ok (Some r) -> return Ok (Some r)
        }

    let paymentHistory : GetPaymentHistory = 

        fun v -> async { 
        
            match! Redis.Get.paymentHistory v with
            | Error msg   -> return Error msg
            | Ok r -> 
                 match r with
                 | None   -> return! Cosmos.Get.paymentHistory v
                 | Some p -> return Ok (Some p)
        }