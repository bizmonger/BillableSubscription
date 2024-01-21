module BillableSubscription.HttpClient.Tests

open System.Configuration
open NUnit.Framework
open BeachMobile.BillableSubscription
open BeachMobile.BillableSubscription.TestAPI.Mock
open BeachMobile.BillableSubscription.DataGateway
open BeachMobile.BillableSubscription.DataGateway.Cosmos

[<Test>]
let ``Sync save registration`` () =

    task {

        // Setup
        let url = @"https://localhost:32768/registration"

        // Test
        let! response = WebGateway.postTo url "registration" ""


        // Verify
        ()
    }