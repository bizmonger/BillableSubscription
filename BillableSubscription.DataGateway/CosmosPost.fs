namespace BeachMobile.BillableSubscription.DataGateway.Cosmos

open System
open System.Net
open BeachMobile.BillableSubscription.Entities
open BeachMobile.BillableSubscription.Operations
open BeachMobile.BillableSubscription.Language
open BeachMobile.BillableSubscription.DataGateway.Common
open BeachMobile.BillableSubscription.DataGateway.Cosmos
open BeachMobile.BillableSubscription.DataGateway.Cosmos.Database

// CosmosDB Permissions
(*
    $resourceGroupName="*****"
    $accountName="*****"
    $principalId="*****" # Often called Object ID
    $dataContributorRoleId="00000000-0000-0000-0000-000000000002"
    az cosmosdb sql role assignment create `
      --account-name $accountName `
      --resource-group $resourceGroupName `
      --scope "/" `
      --principal-id $principalId `
      --role-definition-id $dataContributorRoleId
*)

module Post =

    let registration : RequestRegistration = 
    
        fun v -> async {
            
            try
                let container = Container.get Database.name Container.registration

                let item : RegistrationRequestEntity = {
                    id = Guid.NewGuid() |> string
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

                let registration = status.Registration.Request

                let status : RegistrationStatusEntity = {
                    id = item.id
                    PartitionId = $"{registration.TenantId}:{registration.Plan}"
                    Status = status
                }

                match! container.CreateItemAsync<RegistrationStatusEntity>(status, Microsoft.Azure.Cosmos.PartitionKey(status.PartitionId)) |> Async.AwaitTask with
                | response when response.StatusCode = HttpStatusCode.Created ->
                        
                    return Ok { Registration = receipt
                                Status       = "Pending"
                                Timestamp    = receipt.Timestamp
                              }

                | response -> return Error (response.StatusCode.ToString())

            with ex -> return ex |> toError     
        }

    let payment : SubmitPayment = 
    
        fun v -> async {
            
            try
                let container = Container.get Database.name Container.payments

                let request : PaymentRequestEntity = {
                    id = Guid.NewGuid() |> string
                    PartitionId = v.Subscription.BillablePlan.Plan.Name
                    PaymentRequest = v
                }

                match! container.UpsertItemAsync<PaymentRequestEntity>(request) |> Async.AwaitTask with
                | response when response.StatusCode = HttpStatusCode.Created -> 
                    return Ok { Payment= v; Timestamp= DateTime.UtcNow }

                | response -> return Error (response.StatusCode.ToString())
            
            with ex -> return ex |> toError
        }