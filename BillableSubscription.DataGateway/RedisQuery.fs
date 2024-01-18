namespace BeachMobile.BillableSubscription.DataGateway.Redis

open System
open BeachMobile.BillableSubscription.Operations
open StackExchange.Redis
open Configuration

module Get =

    let subscription : GetRegistrationStatus =

        fun v -> async { 

            let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
            let cache = connection.GetDatabase()
            let result = cache.StringGet("someKey")
            return Error ""
        }

    let paymentHistory : GetPaymentHistory = 

        fun v -> async {

            let! connection = ConnectionMultiplexer.ConnectAsync(ConnectionString.Instance) |> Async.AwaitTask
            let cache = connection.GetDatabase()
            let result = cache.StringGet("someKey")
            return Error ""
        }