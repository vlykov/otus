using MassTransit;
using Microsoft.EntityFrameworkCore;
using InternetShop.Warehouse.Endpoints;
using InternetShop.Warehouse.Infrastructure.Persistence;
using InternetShop.Common.RabbitMq.Options;
using InternetShop.Common.ExceptionHandlers;
using InternetShop.Warehouse;
using InternetShop.Common.Authentication;
using InternetShop.Common.Prometheus;
using InternetShop.Common.Idempotency;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddScoped<IRequestManager, RequestManager<CoreDbContext>>();

builder.Services.AddDbContext<CoreDbContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("Postgres");
    opt.UseNpgsql(connectionString);
});

builder.Services.AddAuthentication("Basic")
        .AddScheme<CustomHeadersAuthenticationOptions, CustomHeadersAuthenticationHandler>("Basic", null);
builder.Services.AddAuthorization();

builder.Services.AddMassTransit(configure =>
{
    configure.AddEntityFrameworkOutbox<CoreDbContext>(_ => 
    {
        // configure which database lock provider to use (Postgres, SqlServer, or MySql)
        _.UsePostgres();

        // enable the bus outbox
        _.UseBusOutbox();
    });

    configure.AddConsumers(typeof(Program).Assembly);
    QueueMappings.MapEventsToQueues();

    configure.UsingRabbitMq((context, configurator) =>
    {
        var rabbitMq = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>()!;

        configurator.Host(rabbitMq.Host, rabbitMq.Port, "/", _ => {
            _.Username(rabbitMq.Username);
            _.Password(rabbitMq.Password);
        });

        //configurator.ConfigureEndpoints(context);
        configurator.MapQueuesToReceiveEndpoints(context);
    });
});

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.UseMetrics();

app.MapGet("/health", () =>
{
    return new { Status = "OK" };
});
app.MapProductEndpoints();
app.MapReservationsEndpoints();

await using var scope = app.Services.CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
await context.Database.MigrateAsync();
await app.RunAsync();
