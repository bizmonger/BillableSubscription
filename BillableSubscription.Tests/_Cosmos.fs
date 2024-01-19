module BillableSubscription.Cosmos.Tests

open System.Net
open System.Configuration
open Microsoft.Azure.Cosmos
open NUnit.Framework
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
        Cosmos.ConnectionString.Instance <- ConfigurationManager.AppSettings["cosmosConnectionString"];

        // Test
        match! someRegistration |> Post.registration with
        | Error msg  -> Assert.Fail msg
        | Ok receipt ->

            // Verify
            let container = Container.get Database.name Partition.registration
            let response  = container.ReadItemAsync<RegistrationRequestEntity>(someRowKey, PartitionKey(receipt.Registration.id)).Result

            Assert.That(response.StatusCode = HttpStatusCode.OK)
    }