using MassTransit;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Orders.Endpoints;
using Otus.Homework.Orders.Infrastructure.Persistence;
using Otus.Homework.Common.Authentication;
using Otus.Homework.Common.ExceptionHandlers;
using Otus.Homework.Common.RabbitMq.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<OrderContext>(opt =>
{
#if DEBUG
    opt.UseInMemoryDatabase("Orders");
#else
    var connectionString = builder.Configuration.GetConnectionString("Postgres");

    opt.UseNpgsql(connectionString);
#endif
});

builder.Services.AddAuthentication("Basic")
        .AddScheme<CustomHeadersAuthenticationOptions, CustomHeadersAuthenticationHandler>("Basic", null);
builder.Services.AddAuthorization();

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumers(typeof(Program).Assembly);

    configure.UsingRabbitMq((context, configurator) =>
    {
        var rabbitMq = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>()!;
        
        configurator.Host(rabbitMq.Host, rabbitMq.Port, "/", _ => {
            _.Username(rabbitMq.Username);
            _.Password(rabbitMq.Password);
        });
        
        configurator.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () =>
{
    return new { Status = "OK" };
});
app.MapApplicationEndpoints();

#if !DEBUG
var context = app.Services.GetRequiredService<OrderContext>();
await context.Database.MigrateAsync();
#endif
await app.RunAsync();
