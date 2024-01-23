// Documentation
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-8.0
// https://blog.jetbrains.com/dotnet/2023/04/25/introduction-to-asp-net-core-minimal-apis/
// Security:
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/security?view=aspnetcore-8.0#enabling-authentication-in-minimal-apps
//
// OpenAPI
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-8.0
// https://medium.com/@gerhardmaree/quickly-create-a-net-6-minimal-api-with-swagger-documentation-720d88db79fb
//
// https://localhost:32768/swagger/v1/swagger.json
// https://localhost:32768/swagger/index.html
//-----------------------------------------------------------------------------------------------------

using Cosmos = BeachMobile.BillableSubscription.DataGateway.Cosmos;
using Redis  = BeachMobile.BillableSubscription.DataGateway.Redis;
using SyncLogic = BeachMobile.BillableSubscription.DataGateway.SyncLogic;
using static BeachMobile.BillableSubscription.Language;

var builder = WebApplication.CreateBuilder(args);
Cosmos.ConnectionString.Instance = builder.Configuration.GetConnectionString("cosmos-connection-string");
Redis .ConnectionString.Instance = builder.Configuration.GetConnectionString("redis-connection-string");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Welcome to Billable Subscriptions");

app.MapPost("/registration", async (RegistrationRequest registration) => {
    
    var result = await Cosmos.Post.registration(registration);

    if (result.IsOk) 
         return Results.Ok(result.ResultValue);
    else return Results.Conflict(result.ErrorValue);
}).WithName("PostRegistration")
  .WithOpenApi()
  .WithTags("RegistrationGroup");

app.MapPost("/get_registration_status", async (RegistrationReceipt receipt) => {

    var result = await SyncLogic.Query.status(receipt);

    if (result.IsOk)
         return Results.Ok(result.ResultValue);
    else return Results.Conflict(result.ErrorValue);
}).WithName("GetRegistrationStatus")
  .WithOpenApi()
  .WithTags("RegistrationGroup");

app.MapPost("/payment", async (PaymentRequest payment) => {

    var result = await Cosmos.Post.payment(payment);

    if (result.IsOk)
         return Results.Ok(result.ResultValue);
    else return Results.Conflict(result.ErrorValue);
}).WithName("PostPaymentRequest")
  .WithOpenApi()
  .WithTags("PaymentsGroup");

app.MapPost("/get_payment_history", async (PaymentHistoryRequest request) => {

    var result = await SyncLogic.Query.paymentHistory(request.SubscriptionId);

    if (result.IsOk)
         return Results.Ok(result.ResultValue);
    else return Results.Conflict(result.ErrorValue);
}).WithName("GetPaymentHistory")
  .WithOpenApi()
  .WithTags("PaymentsGroup");

app.Run();