module BillableSubscription.Tests

open System.Configuration
open Microsoft.Azure.Cosmos
open Azure.Identity
open NUnit.Framework
open BeachMobile.BillableSubscription.TestAPI.Mock
open BeachMobile.BillableSubscription.Entities
open System.Net

[<Ignore("")>]
[<Test>]
let ``add registration`` () =

    // Setup
    let connectionString = ConfigurationManager.AppSettings["connectionString"];
    let client     = new CosmosClient(connectionString, DefaultAzureCredential())
    let database   = client.GetDatabase("beachmobile-db")
    let container  = database.GetContainer("registration");

    let request : RegistrationRequestEntity = {
        id = someRowKey
        RegistrationRequest = someRegistration
    }

    // Test
    container.UpsertItemAsync<RegistrationRequestEntity>(request).Result |> ignore

    // Verify
    let response = container.ReadItemAsync<RegistrationRequestEntity>(someRowKey, PartitionKey(request.id)).Result
    Assert.That(response.StatusCode = HttpStatusCode.OK)