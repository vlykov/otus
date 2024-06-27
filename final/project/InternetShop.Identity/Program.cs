using MassTransit;
using Microsoft.EntityFrameworkCore;
using InternetShop.Common.ExceptionHandlers;
using InternetShop.Common.RabbitMq.Options;
using InternetShop.Identity;
using InternetShop.Identity.Endpoints;
using InternetShop.Identity.Infrastructure.Persistence;
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

// Добавляем IDistributedMemoryCache (нужен в частности для работы сессий).
builder.Services.AddDistributedMemoryCache();
// Добавляем сервисы сессии.
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "sakurlyk.identity.session";
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(1);
});

builder.Services.AddMassTransit(configure =>
{
    configure.AddEntityFrameworkOutbox<CoreDbContext>(_ =>
    {
        // configure which database lock provider to use (Postgres, SqlServer, or MySql)
        _.UsePostgres();

        // enable the bus outbox
        _.UseBusOutbox();
    });

    //configure.AddConsumers(typeof(Program).Assembly);
    QueueMappings.MapEventsToQueues();

    configure.UsingRabbitMq((context, configurator) =>
    {
        var rabbitMq = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>()!;

        configurator.Host(rabbitMq.Host, rabbitMq.Port, "/", _ => {
            _.Username(rabbitMq.Username);
            _.Password(rabbitMq.Password);
        });

        //configurator.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseSwagger();
app.UseSwaggerUI();
// Добавляем middleware для работы с сессиями.
app.UseSession();
app.UseMetrics();

app.MapGet("/health", () =>
{
    return new { Status = "OK" };
});
app.MapUserEndpoints();

await using var scope = app.Services.CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
await context.Database.MigrateAsync();
await app.RunAsync();
