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
            
            try
                let container = Container.get Database.name Container.registration

                let item : RegistrationRequestEntity = {
                    id = Guid.NewGuid() |> string
                    PartitionId = "hello-world"
                    RegistrationRequest = v
                }

                let receipt : RegistrationReceipt = {
                    id = item.id
                    Request   = v
                    Timestamp = DateTime.UtcNow
                }

                let status : RegistrationStatus = {
                    Registration = receipt
                    Status       = "pending"
                    Timestamp    = DateTime.UtcNow
                }

                let status : RegistrationStatusEntity = {
                    id = item.id
                    Status = status
                }

                match! container.CreateItemAsync<RegistrationStatusEntity>(status, Microsoft.Azure.Cosmos.PartitionKey(item.PartitionId)) |> Async.AwaitTask with
                | response when response.StatusCode = HttpStatusCode.Created ->
                        
                    return Ok { Registration = receipt
                                Status       = "Pending"
                                Timestamp    = receipt.Timestamp
                                }

                | response -> return Error (response.StatusCode.ToString())

            with ex -> 
                let msg = ex.GetBaseException().Message
                return Error msg       
        }

    let payment : SubmitPayment = 
    
        fun v -> async {
            
            let container = Container.get Database.name Container.payments

            let request : PaymentRequestEntity = {
                id = Guid.NewGuid() |> string
                PartitionId = "hellow-world"
                PaymentRequest = v
            }

            match! container.UpsertItemAsync<PaymentRequestEntity>(request) |> Async.AwaitTask with
            | response when response.StatusCode = HttpStatusCode.OK -> 
                return Ok { Payment= v; Timestamp= DateTime.UtcNow }

            | response -> return Error (response.StatusCode.ToString())
        }