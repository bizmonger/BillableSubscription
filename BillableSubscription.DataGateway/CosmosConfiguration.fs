namespace BillableSubscription.DataGateway.Cosmos

type ConnectionString() = static member val Instance = ""

module Partition =

    let registration = "Registration"
    let payments     = "Payments"

module Database =

    let name = "beachmobile-db"