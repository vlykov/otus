using Microsoft.EntityFrameworkCore;
using Otus.Homework.Orders.Endpoints;
using Otus.Homework.Orders.Infrastructure.Persistence;
using Otus.Homework.Common.ExceptionHandlers;
using Otus.Homework.Common.Idempotency;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddScoped<IRequestManager, RequestManager<OrderContext>>();

builder.Services.AddDbContext<OrderContext>(opt =>
{
#if DEBUG
    opt.UseInMemoryDatabase("Orders");
#else
    var connectionString = builder.Configuration.GetConnectionString("Postgres");

    opt.UseNpgsql(connectionString);
#endif
});

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseSwagger();
app.UseSwaggerUI();

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
