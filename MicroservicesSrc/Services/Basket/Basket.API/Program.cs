using Basket.API.gRPC;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
{
    var options = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
    return ConnectionMultiplexer.Connect(options);
});

// Grpc
builder.Services.AddScoped<DiscountGrpcProxy>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
{
    opt.Address = new Uri(builder.Configuration["GrpcSettings:DiscountServiceUrl"]); 
});

// Common
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Configure Rabbitmq via MassTransit
builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["Queue:RabbitMQHost"]);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
