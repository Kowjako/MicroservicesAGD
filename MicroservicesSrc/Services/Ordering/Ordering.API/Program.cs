using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Migrator;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterApplicationServices();
builder.Services.RegisterInfrastructureServices(builder.Configuration);

builder.Services.AddMassTransit(cfg =>
{
    // Register subscriber for rabbitmq event published from basket API
    cfg.AddConsumer<BasketCheckoutConsumer>();

    cfg.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["Queue:RabbitMQHost"]);
        configurator.ReceiveEndpoint(Constants.BASKET_CHECKOUT_QUEUE, x =>
        {
            // tie this consumer to subscribe to specified queue
            x.ConfigureConsumer<BasketCheckoutConsumer>(context);
        });
    });
});

var app = builder.Build();

await app.MigrateMcrDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
