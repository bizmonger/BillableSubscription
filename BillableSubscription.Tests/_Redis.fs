module BillableSubscription.Redis.Tests

open System.Configuration
open NUnit.Framework
open BeachMobile.BillableSubscription.TestAPI.Mock
open BeachMobile.BillableSubscription.DataGateway.Redis
open StackExchange.Redis

type ConnectionString = BeachMobile.BillableSubscription.DataGateway.Redis.ConnectionString

[<Ignore("")>]
[<Test>]
let ``cache registration`` () =

    async {
    
        // Setup
        ConnectionString.Instance <- ConfigurationManager.AppSettings["redisConnectionString"];
        let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
        let cache = connection.GetDatabase()

        // Test
        match! someRegistrationReceipt |> Post.registration with
        | Error msg  -> Assert.Fail msg
        | Ok status ->

            // Verify
            let registration = cache.StringGet(KeyFor.registration status.Registration.id)
            Assert.That registration.HasValue

        // Teardown
        do! connection.CloseAsync() |> Async.AwaitTask
    }