namespace BeachMobile.BillableSubscription.TestAPI

open System
open BeachMobile.BillableSubscription.Language

module Mock =

    let someRowKey = "some_row_key"
    let someHost   = "some business"

    let someRegistration : RegistrationRequest = {
        TenantId  = "BeachMobile"
        FirstName = "some first name"
        LastName  = "some last name"
        Phone     = "some phone"
        Email     = "some email"
        Plan      = "some plan"
    }

    let someRegistrationReceipt : RegistrationReceipt = {
        id = someRowKey
        Request   = someRegistration
        Timestamp = DateTime.UtcNow
    }

    let someRegistrationStatus : RegistrationStatus = {
        Registration = someRegistrationReceipt
        Status    = "Pending"
        Timestamp = someRegistrationReceipt.Timestamp
    }

    let somePlan : Plan = {
        Name        = "some plan"
        Description = "some plan description"
    }

    let someBillablePlan : BillablePlan = {
        Plan      = somePlan
        Price     = 8.99
        Frequency = "monthly"
    }

    let someSubscription : Subscription = {
        Registration = someRegistrationReceipt
        BillablePlan         = someBillablePlan
        Started      = DateTime.UtcNow
    }

    let somePayment : PaymentRequest = {
        Subscription = someSubscription
        Amount       = 9.99
    }