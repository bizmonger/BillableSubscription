namespace BeachMobile.BillableSubscription.TestAPI

open BeachMobile.BillableSubscription.Language

module Mock =

    let someConnectionString = "some_connection_string"

    let someRowKey       = "some_row_key"
    let somePartitionKey = "some_partition_key"

    let someRegistration : RegistrationRequest = {
        FirstName = "some first name"
        LastName  = "some last name"
        Phone     = "some phone"
        Email     = "some email"
    }