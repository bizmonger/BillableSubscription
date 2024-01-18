namespace BeachMobile.BillableSubscription.DataGateway.Sync

open BeachMobile.BillableSubscription.Operations

module Post =

    let registration : RequestRegistration =

        fun v -> async { return Error "" }

    let Payment : SubmitPayment = 
    
        fun v -> async { return Error "" }