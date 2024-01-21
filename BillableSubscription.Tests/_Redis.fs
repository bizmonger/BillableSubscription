module BillableSubscription.Redis.Tests

open System.Configuration
open NUnit.Framework
open BeachMobile.BillableSubscription.TestAPI.Mock
open BeachMobile.BillableSubscription.DataGateway.Redis
open StackExchange.Redis

type ConnectionString = BeachMobile.BillableSubscription.DataGateway.Redis.ConnectionString

[<Test>]
let ``cache registration`` () =

    async {
    
        // Setup
        ConnectionString.Instance <- ConfigurationManager.AppSettings["redisConnectionString"];
        let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
        let cache = connection.GetDatabase()

        // Test
        match! someRegistrationStatus |> Post.registration with
        | Error msg  -> Assert.Fail msg
        | Ok () ->

            // Verify
            let receipt      = someRegistrationStatus.Registration.Request
            let registration = cache.StringGet(KeyFor.registrationStatus(receipt.TenantId, receipt.Plan))
            Assert.That registration.HasValue

        // Teardown
        do! connection.CloseAsync() |> Async.AwaitTask
    }