namespace BeachMobile.BillableSubscription.DataGateway.Redis

module KeyFor =
    
    let payment(subscriptionId)            = $"Payment:{subscriptionId}"
    let registrationStatus(subscriptionId) = $"RegistrationStatus:{subscriptionId}"
    let paymentHistory(subscriptionId)     = $"PaymentHistory:{subscriptionId}"