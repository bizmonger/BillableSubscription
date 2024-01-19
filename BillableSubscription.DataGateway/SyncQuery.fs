﻿namespace BeachMobile.BillableSubscription.DataGateway.Sync

open BeachMobile.BillableSubscription.Language
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.DataGateway

module Query =

    let status : GetRegistrationStatus =

        fun v -> async {

            let cache(status:RegistrationStatus) =

                async {
                
                    match! Redis.Post.registration status.Registration with
                    | Error msg -> return Error msg
                    | Ok r      -> return Ok (Some r)
                }
        
            match! Redis.Get.status v with
            | Error msg   -> return Error msg
            | Ok (Some r) -> return Ok (Some r)
            | Ok None -> 

                match! Cosmos.Get.status v with
                | Error msg   -> return Error msg
                | Ok None     -> return Ok None
                | Ok (Some r) -> return! cache r
        }

    let paymentHistory : GetPaymentHistory = 

        fun v -> async {

            let cache items =

                async {
                
                    match! Redis.Post.paymentHistory { SubscriptionId=v; Payments= items} with
                    | Error msg -> return Error msg
                    | Ok ()     -> return Ok None
                }
        
            match! Redis.Get.paymentHistory v with
            | Error msg   -> return Error msg
            | Ok (Some r) -> return Ok (Some r)
            | Ok None -> 

                match! Cosmos.Get.paymentHistory v with
                | Error msg   -> return Error msg
                | Ok None     -> return! cache Seq.empty
                | Ok (Some r) -> return! cache r
        }