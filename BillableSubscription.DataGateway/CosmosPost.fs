namespace BillableSubscription.DataGateway.Cosmos

open System
open System.Net
open Azure.Identity
open Microsoft.Azure.Cosmos
open BeachMobile.BillableSubscription.Entities
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.Language

module Post =

    let Payment : SubmitPayment = 
    
        fun v ->

            async {
            
                let client    = new CosmosClient(ConnectionString.Instance, DefaultAzureCredential())
                let database  = client.GetDatabase(Database.name)
                let container = database.GetContainer(Partition.payments);

                let request : PaymentRequestEntity = {
                    PartitionKey = Partition.payments
                    id = Guid.NewGuid() |> string
                    PaymentRequest = v
                }

                match! container.UpsertItemAsync<PaymentRequestEntity>(request) |> Async.AwaitTask with
                | response when response.StatusCode = HttpStatusCode.OK -> 
                    return Ok { Payment   = v; Timestamp = DateTime.UtcNow }

                | response -> return Error (response.StatusCode.ToString())
            }

    let Registration : RequestRegistration = 
    
        fun v ->

            async {
            
                let client    = new CosmosClient(ConnectionString.Instance, DefaultAzureCredential())
                let database  = client.GetDatabase(Database.name)
                let container = database.GetContainer(Partition.registration);

                let request : RegistrationRequestEntity = {
                    PartitionKey = Partition.registration
                    id = Guid.NewGuid() |> string
                    RegistrationRequest = v
                }

                match! container.UpsertItemAsync<RegistrationRequestEntity>(request) |> Async.AwaitTask with
                | response when response.StatusCode = HttpStatusCode.OK ->
                    return Ok { id = request.id; Request = v; Timestamp = DateTime.UtcNow }

                | response -> return Error (response.StatusCode.ToString())
            }