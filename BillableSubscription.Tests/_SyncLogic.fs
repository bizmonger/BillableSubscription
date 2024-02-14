module BillableSubscription.Sync.Tests

open System.Configuration
open NUnit.Framework
open Microsoft.Azure.Cosmos
open Azure.Identity
open StackExchange.Redis
open BeachMobile.BillableSubscription.TestAPI.Mock
open BeachMobile.BillableSubscription.DataGateway
open BeachMobile.BillableSubscription.DataGateway.Cosmos
open BeachMobile.BillableSubscription.DataGateway.Redis

[<Test>]
let ``Sync save registration`` () =

    async {
    
        // Setup
        let cosmosConnectionString = ConfigurationManager.AppSettings["cosmosConnectionString"];
        let redisConnectionString = ConfigurationManager.AppSettings["RedisConnectionString"];

        let cosmosClient = new CosmosClient(cosmosConnectionString, DefaultAzureCredential())
        let! multiplexer = ConnectionMultiplexer.ConnectAsync(redisConnectionString) |> Async.AwaitTask

        let connection : SyncConnection = {
            Multiplexer  = multiplexer
            CosmosClient = cosmosClient
        }

        // Test
        match! connection.Multiplexer |> Post.registration someRegistrationStatus |> Async.AwaitTask with
        | Error msg  -> Assert.Fail msg
        | Ok receipt ->

            // Verify
            match! connection |> SyncLogic.Query.status receipt |> Async.AwaitTask with
            | Error msg -> Assert.Fail msg
            | Ok status -> Assert.That(status.Value.Registration.Request = someRegistration)
    }

[<Ignore("")>]
[<Test>]
let ``save payment`` () =

    async {
    
        // Setup
        let cosmosConnectionString = ConfigurationManager.AppSettings["cosmosConnectionString"];
        let redisConnectionString = ConfigurationManager.AppSettings["RedisConnectionString"];

        let cosmosClient = new CosmosClient(cosmosConnectionString, DefaultAzureCredential())
        let! multiplexer = ConnectionMultiplexer.ConnectAsync(redisConnectionString) |> Async.AwaitTask

        let connection : SyncConnection = {
            Multiplexer  = multiplexer
            CosmosClient = cosmosClient
        }

        // Test
        match! multiplexer |> Post.payment somePayment |> Async.AwaitTask with
        | Error msg  -> Assert.Fail msg
        | Ok success ->

            // Verify
            let subscriptionId = success.Payment.Subscription.Registration.id

            match! connection |> SyncLogic.Query.paymentHistory subscriptionId |> Async.AwaitTask with
            | Error msg   -> Assert.Fail msg
            | Ok None     -> Assert.Fail "no history"
            | Ok (Some h) -> Assert.That((h |> Seq.head).Payment = somePayment)
    }