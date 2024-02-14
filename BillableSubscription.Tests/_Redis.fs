module BillableSubscription.Redis.Tests

open System.Configuration
open NUnit.Framework
open BeachMobile.BillableSubscription.TestAPI.Mock
open BeachMobile.BillableSubscription.DataGateway.Redis
open StackExchange.Redis

[<Test>]
let ``cache registration`` () =

    async {
    
        // Setup
        let redisConnectionString = ConfigurationManager.AppSettings["redisConnectionString"]
        let! connection = ConnectionMultiplexer.ConnectAsync(redisConnectionString) |> Async.AwaitTask
        let cache = connection.GetDatabase()

        // Test
        match! connection |> Post.registration someRegistrationStatus |> Async.AwaitTask with
        | Error msg  -> Assert.Fail msg
        | Ok receipt ->

            // Verify
            let registration = cache.StringGet(KeyFor.registrationStatus(receipt.Request.TenantId, receipt.Request.Plan))
            Assert.That registration.HasValue

        // Teardown
        do! connection.CloseAsync() |> Async.AwaitTask
    }