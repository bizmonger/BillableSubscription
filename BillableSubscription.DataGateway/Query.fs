namespace BillableSubscription.DataGateway

open BeachMobile.BillableSubscription.Operations

module Get =

    let subscription   : GetSubscriptionStatus = fun _ -> Error ""
    let paymentHistory : GetPaymentHistory     = fun _ -> Error ""