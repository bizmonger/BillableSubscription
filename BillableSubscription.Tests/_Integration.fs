module BillableSubscription.Tests

open System.Net
open System.Configuration
open Microsoft.Azure.Cosmos
open NUnit.Framework
open BeachMobile.BillableSubscription.TestAPI.Mock
open BeachMobile.BillableSubscription.Entities
open BeachMobile.BillableSubscription.DataGateway.Cosmos
open BeachMobile.BillableSubscription.DataGateway.Cosmos.Database

[<Ignore("")>]
[<Test>]
let ``add registration`` () =

    // Setup
    ConnectionString.Instance <- ConfigurationManager.AppSettings["connectionString"];
    let container = container Database.name Partition.registration

    let request : RegistrationRequestEntity = {
        PartitionKey = Partition.registration
        id = someRowKey
        RegistrationRequest = someRegistration
    }

    // Test
    container.UpsertItemAsync<RegistrationRequestEntity>(request).Result |> ignore

    // Verify
    let response = container.ReadItemAsync<RegistrationRequestEntity>(someRowKey, PartitionKey(request.id)).Result
    Assert.That(response.StatusCode = HttpStatusCode.OK)