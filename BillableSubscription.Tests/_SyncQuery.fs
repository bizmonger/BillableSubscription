module BillableSubscription.Sync.Tests

open System.Configuration
open NUnit.Framework
open BeachMobile.BillableSubscription.TestAPI.Mock
open BeachMobile.BillableSubscription.DataGateway
open BeachMobile.BillableSubscription.DataGateway.Cosmos

[<Ignore("")>]
[<Test>]
let ``save registration`` () =

    async {
    
        // Setup
        Cosmos.ConnectionString.Instance <- ConfigurationManager.AppSettings["cosmosConnectionString"];
        Redis. ConnectionString.Instance <- ConfigurationManager.AppSettings["RedisConnectionString"];

        // Test
        match! someRegistration |> Post.registration with
        | Error msg  -> Assert.Fail msg
        | Ok receipt ->

            // Verify
            match! SyncLogic.Query.status receipt.Registration with
            | Error msg -> Assert.Fail msg
            | Ok status -> Assert.That(status.Value.Registration.Request = someRegistration)
    }