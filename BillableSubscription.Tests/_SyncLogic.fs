module BillableSubscription.Sync.Tests

open System.Configuration
open NUnit.Framework
open BeachMobile.BillableSubscription.TestAPI.Mock
open BeachMobile.BillableSubscription.DataGateway
open BeachMobile.BillableSubscription.DataGateway.Cosmos

[<Test>]
let ``Sync save registration`` () =

    async {
    
        // Setup
        Cosmos.ConnectionString.Instance <- ConfigurationManager.AppSettings["cosmosConnectionString"];
        Redis. ConnectionString.Instance <- ConfigurationManager.AppSettings["RedisConnectionString"];

        // Test
        match! someRegistration |> Post.registration |> Async.AwaitTask with
        | Error msg  -> Assert.Fail msg
        | Ok receipt ->

            // Verify
            match! SyncLogic.Query.status receipt.Registration |> Async.AwaitTask with
            | Error msg -> Assert.Fail msg
            | Ok status -> Assert.That(status.Value.Registration.Request = someRegistration)
    }

[<Ignore("")>]
[<Test>]
let ``save payment`` () =

    async {
    
        // Setup
        Cosmos.ConnectionString.Instance <- ConfigurationManager.AppSettings["cosmosConnectionString"];
        Redis. ConnectionString.Instance <- ConfigurationManager.AppSettings["RedisConnectionString"];

        // Test
        match! somePayment |> Post.payment |> Async.AwaitTask with
        | Error msg  -> Assert.Fail msg
        | Ok success ->

            // Verify
            let subscriptionId = success.Payment.Subscription.Registration.id

            match! SyncLogic.Query.paymentHistory subscriptionId |> Async.AwaitTask with
            | Error msg   -> Assert.Fail msg
            | Ok None     -> Assert.Fail "no history"
            | Ok (Some h) -> Assert.That((h |> Seq.head).Payment = somePayment)
    }