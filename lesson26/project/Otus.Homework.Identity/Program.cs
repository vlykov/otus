using MassTransit;
using Microsoft.EntityFrameworkCore;
using Otus.Homework.Common.ExceptionHandlers;
using Otus.Homework.Common.RabbitMq.Options;
using Otus.Homework.Identity.Endpoints;
using Otus.Homework.Identity.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<UserContext>(opt =>
{
#if DEBUG
    opt.UseInMemoryDatabase("Users");
#else
    var connectionString = builder.Configuration.GetConnectionString("Postgres");

    opt.UseNpgsql(connectionString);
#endif
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
    //configure.AddConsumers(typeof(Program).Assembly);

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
// Добавляем middleware для работы с сессиями.
app.UseSession();

app.MapGet("/health", () =>
{
    return new { Status = "OK" };
});
app.MapUserEndpoints();

#if !DEBUG
var context = app.Services.GetRequiredService<UserContext>();
await context.Database.MigrateAsync();
#endif
await app.RunAsync();
