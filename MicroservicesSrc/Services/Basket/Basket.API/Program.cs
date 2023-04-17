using Basket.API.gRPC;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
{
    var options = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
    return ConnectionMultiplexer.Connect(options);
});
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
{
    opt.Address = new Uri(builder.Configuration["GrpcSettings:DiscountServiceUrl"]);
});
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcProxy>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
