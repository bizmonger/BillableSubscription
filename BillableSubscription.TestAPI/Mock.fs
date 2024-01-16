namespace BeachMobile.BillableSubscription.TestAPI

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