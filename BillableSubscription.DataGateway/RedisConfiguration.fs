namespace BeachMobile.BillableSubscription.DataGateway.Redis

module KeyFor =
    
    let payment(subscriptionId)            = $"Payment:{subscriptionId}"
    let registrationStatus(tenantId,plan)  = $"{tenantId}:{plan}"
    let paymentHistory(subscriptionId)     = $"PaymentHistory:{subscriptionId}"