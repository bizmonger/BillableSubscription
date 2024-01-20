// Documentation
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-8.0
// https://blog.jetbrains.com/dotnet/2023/04/25/introduction-to-asp-net-core-minimal-apis/
//-----------------------------------------------------------------------------------------------------

using BeachMobile.BillableSubscription.DataGateway.Cosmos;
using static BeachMobile.BillableSubscription.Language;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/registration", async (RegistrationRequest registration) => {
    
    var result = await Post.registration(registration);

    if (result.IsOk) 
         return Results.Ok(result.ResultValue);
    else return Results.Conflict(result.ErrorValue);
});

app.Run();