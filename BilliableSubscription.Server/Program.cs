// Documentation
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-8.0
// https://blog.jetbrains.com/dotnet/2023/04/25/introduction-to-asp-net-core-minimal-apis/
// Security:
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/security?view=aspnetcore-8.0#enabling-authentication-in-minimal-apps
//-----------------------------------------------------------------------------------------------------

using BeachMobile.BillableSubscription.DataGateway.Cosmos;
using static BeachMobile.BillableSubscription.Language;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "welcome to Billable Subscriptions");

app.MapPost("/registration", async (RegistrationRequest registration) => {
    
    var result = await Post.registration(registration);

    if (result.IsOk) 
         return Results.Ok(result.ResultValue);
    else return Results.Conflict(result.ErrorValue);
});

app.MapPost("/registration_status", async (RegistrationReceipt receipt) => {

    var result = await Get.status(receipt);

    if (result.IsOk)
         return Results.Ok(result.ResultValue);
    else return Results.Conflict(result.ErrorValue);
});

app.MapPost("/payment", async (PaymentRequest payment) => {

    var result = await Post.payment(payment);

    if (result.IsOk)
         return Results.Ok(result.ResultValue);
    else return Results.Conflict(result.ErrorValue);
});

app.MapPost("/payment_history", async (PaymentHistoryRequest request) => {

    var result = await Get.paymentHistory(request.SubscriptionId);

    if (result.IsOk)
         return Results.Ok(result.ResultValue);
    else return Results.Conflict(result.ErrorValue);
});

app.Run();