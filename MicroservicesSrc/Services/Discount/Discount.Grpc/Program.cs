using Discount.Grpc.Extensions;
using Discount.Grpc.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

var app = builder.Build();
await app.MigratePgDatabase();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
