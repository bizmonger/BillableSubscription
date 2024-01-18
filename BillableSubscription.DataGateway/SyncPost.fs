namespace BeachMobile.BillableSubscription.DataGateway.Sync

open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.DataGateway

module Post =

    let registration : RequestRegistration =

        fun v -> async { 
            match! Cosmos.Post.Registration v with
            | Error msg -> return Error msg
            | Ok _ -> return Error "" 
            
        }

    let Payment : SubmitPayment = 
    
        fun v -> async { return Error "" 
        
        }