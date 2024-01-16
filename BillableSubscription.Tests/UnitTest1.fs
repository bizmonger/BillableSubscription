module BillableSubscription.Tests

open Microsoft.Azure.Cosmos
open Azure.Identity
open NUnit.Framework
open BeachMobile.BillableSubscription.TestAPI.Mock
open BeachMobile.BillableSubscription.Entities

[<Test>]
let ``registration request`` () =

    // Setup
    let credential = DefaultAzureCredential()
    let client     = new CosmosClient(someConnectionString, credential)
    let database   = client.GetDatabase("some_database")
    let container  = database.GetContainer("some_container");

    let request : RegistrationRequestEntity = {
        id = someRowKey
        RegistrationRequest = someRegistration
    }

    // Test
    container.UpsertItemAsync<RegistrationRequestEntity>(request, PartitionKey(somePartitionKey)) |> ignore

    // Verify
    let document = container.ReadItemAsync<RegistrationRequestEntity>(someRowKey, PartitionKey(somePartitionKey)).Result
    Assert.Pass()