namespace BillableSubscription.DataGateway

open BeachMobile.BillableSubscription.Operations

module Post =

    let Payment      : SubmitPayment       = fun _ -> Error ""
    let Registration : RequestRegistration = fun _ -> Error ""