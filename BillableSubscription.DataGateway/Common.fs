namespace BeachMobile.BillableSubscription.DataGateway

module Common =

    let toError (ex:exn) =
        let msg = ex.GetBaseException().Message
        Error msg    