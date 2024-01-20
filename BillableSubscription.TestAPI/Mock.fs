namespace BeachMobile.BillableSubscription.TestAPI

open System
open BeachMobile.BillableSubscription.Language

module Mock =

    let someRowKey = "some_row_key"
    let somePartitionKey = "id"

    let someRegistration : RegistrationRequest = {
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
        Plan         = someBillablePlan
        Started      = DateTime.UtcNow
    }

    let somePayment : PaymentRequest = {
        Subscription = someSubscription
        Amount       = 9.99
    }