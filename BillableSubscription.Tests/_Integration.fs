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

    async {
    
        // Setup
        ConnectionString.Instance <- ConfigurationManager.AppSettings["cosmosConnectionString"];

        // Test
        match! someRegistration |> Post.Registration with
        | Error msg  -> Assert.Fail msg
        | Ok receipt ->

            // Verify
            let container = container Database.name Partition.registration
            let response = container.ReadItemAsync<RegistrationRequestEntity>(someRowKey, PartitionKey(receipt.id)).Result

            Assert.That(response.StatusCode = HttpStatusCode.OK)
    }