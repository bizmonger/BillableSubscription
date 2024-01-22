module BillableSubscription.HttpClient.Tests

open NUnit.Framework
open BeachMobile.BillableSubscription
open BeachMobile.BillableSubscription.TestAPI.Mock

[<Test>]
let ``Sync save registration`` () =

    task {

        // Setup
        let url = @"https://localhost:32772/"

        // Test
        let! response = WebGateway.get url "registration" someRegistration

        // Verify
        ()
    }