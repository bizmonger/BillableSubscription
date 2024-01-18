namespace BeachMobile.BillableSubscription.DataGateway.Sync

open BeachMobile.BillableSubscription.Operations

module Query =

    let status : GetRegistrationStatus =

        fun v -> async { return Error "" }

    let paymentHistory : GetPaymentHistory = 

        fun v -> async { return Error "" }