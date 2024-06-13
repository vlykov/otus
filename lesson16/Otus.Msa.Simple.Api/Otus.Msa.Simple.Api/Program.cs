using Microsoft.EntityFrameworkCore;
using Otus.Msa.Simple.Api.Endpoints;
using Otus.Msa.Simple.Api.ExceptionHandlers;
using Otus.Msa.Simple.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<UserContext>(opt =>
{
    //opt.UseInMemoryDatabase("Users");

    var connectionString = builder.Configuration.GetConnectionString("Postgres");

    opt.UseNpgsql(connectionString);
});

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/health", () =>
{
    return new { Status = "OK" };
});
app.MapUserEndpoints();

var context = app.Services.GetRequiredService<UserContext>();
await context.Database.MigrateAsync();
await app.RunAsync();
