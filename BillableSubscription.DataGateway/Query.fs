namespace BillableSubscription.DataGateway

open BeachMobile.BillableSubscription.Operations

module Get =

    let subscription   : GetSubscriptionStatus = fun _ -> async { return Error "" }
    let paymentHistory : GetPaymentHistory     = fun _ -> async { return Error "" }