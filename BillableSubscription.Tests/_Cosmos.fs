module BillableSubscription.Cosmos.Tests

open System.Net
open System.Configuration
open Microsoft.Azure.Cosmos
open NUnit.Framework
open Azure.Identity
open BeachMobile.BillableSubscription.TestAPI.Mock
open BeachMobile.BillableSubscription.Entities
open BeachMobile.BillableSubscription.DataGateway
open BeachMobile.BillableSubscription.DataGateway.Cosmos
open BeachMobile.BillableSubscription.DataGateway.Cosmos.Database

[<Ignore("")>]
[<Test>]
let ``save registration`` () =

    async {
    
        // Setup
        let cosmosConnectionString = ConfigurationManager.AppSettings["cosmosConnectionString"];
        let client = new CosmosClient(cosmosConnectionString, DefaultAzureCredential())

        // Test
        match! client |> Post.registration someRegistration |> Async.AwaitTask with
        | Error msg  -> Assert.Fail msg
        | Ok receipt ->

            // Verify
            let container = client |> Container.get Database.name Container.registration
            let response  = container.ReadItemAsync<RegistrationRequestEntity>(someRowKey, PartitionKey(receipt.Registration.id)).Result

            Assert.That(response.StatusCode = HttpStatusCode.OK)
    }