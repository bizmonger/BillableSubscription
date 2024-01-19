namespace BeachMobile.BillableSubscription.DataGateway.Cosmos

open System
open System.Net
open BeachMobile.BillableSubscription.Entities
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.Language
open BeachMobile.BillableSubscription.DataGateway.Cosmos
open BeachMobile.BillableSubscription.DataGateway.Cosmos.Database

module Post =

    let registration : RequestRegistration = 
    
        fun v -> async {
            
            let container = Container.get Database.name Partition.registration

            let registration : RegistrationRequestEntity = {
                PartitionKey = Partition.registration
                id = Guid.NewGuid() |> string
                RegistrationRequest = v
            }

            match! container.UpsertItemAsync<RegistrationRequestEntity>(registration) |> Async.AwaitTask with
            | response when response.StatusCode = HttpStatusCode.OK ->

                let receipt : RegistrationReceipt = {
                    id = registration.id
                    Request   = v
                    Timestamp = DateTime.UtcNow
                }
                    
                return Ok { Registration = receipt
                            Status       = "Pending"
                            Timestamp    = receipt.Timestamp
                            }

            | response -> return Error (response.StatusCode.ToString())
        }

    let payment : SubmitPayment = 
    
        fun v -> async {
            
            let container = Container.get Database.name Partition.payments

            let request : PaymentRequestEntity = {
                PartitionKey = Partition.payments
                id = Guid.NewGuid() |> string
                PaymentRequest = v
            }

            match! container.UpsertItemAsync<PaymentRequestEntity>(request) |> Async.AwaitTask with
            | response when response.StatusCode = HttpStatusCode.OK -> 
                return Ok { Payment= v; Timestamp= DateTime.UtcNow }

            | response -> return Error (response.StatusCode.ToString())
        }