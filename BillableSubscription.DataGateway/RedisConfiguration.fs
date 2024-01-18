namespace BeachMobile.BillableSubscription.DataGateway.Redis

open StackExchange.Redis

module Configuration =

    type ConnectionString() = static member val Instance = "" with get,set