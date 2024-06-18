using Microsoft.EntityFrameworkCore;
using Otus.Msa.Profile.Api.Endpoints;
using Otus.Msa.Profile.Api.ExceptionHandlers;
using Otus.Msa.Profile.Api.Infrastructure.Persistence;
using Otus.Msa.Profile.Api.Middlewares;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddScoped<AuthorizationContext>();

builder.Services.AddDbContext<UserProfileContext>(opt =>
{
    //opt.UseInMemoryDatabase("UserProfiles");

    var connectionString = builder.Configuration.GetConnectionString("Postgres");

    opt.UseNpgsql(connectionString);
});

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseSwagger();
app.UseSwaggerUI();

// Suppresses the registration of the default sample metrics from the default registry.
Metrics.SuppressDefaultMetrics();
Metrics.ConfigureMeterAdapter(options => {
    //// Filter metrics published by the .Net Meter API
    //options.InstrumentFilterPredicate = (instrument) =>
    //         instrument.Name.StartsWith("microsoft_aspnetcore_hosting_http_server_request_duration") ||
    //         instrument.Name.StartsWith("microsoft_aspnetcore_server_kestrel_kestrel_connection_duration");

    options.ResolveHistogramBuckets = (instrument) => Histogram.ExponentialBuckets(0.5, 2.0, 10);
});
// Capture metrics about all received HTTP requests.
app.UseHttpMetrics(options =>
{
    options.RequestDuration.Histogram = Metrics.CreateHistogram(
        name: "http_request_duration_seconds",
        help: "The duration of HTTP requests processed by an ASP.NET Core application.",
        labelNames: ["code", "method", "endpoint", "custom_host"],
        new HistogramConfiguration
        {
            Buckets = [0.5, 0.95, 0.99]
        });
    options.AddCustomLabel("custom_host", httpContext => httpContext.Request.Host.Host);
    options.RequestCount.Enabled = false;
    options.InProgress.Enabled = false;
});
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/v1"), app =>
{
    app.UseCustomAuthorization();
});
// Enable the /metrics page to export Prometheus metrics.
app.MapMetrics("/metrics");

app.MapGet("/health", () =>
{
    return new { Status = "OK" };
});
app.MapUserProfilesEndpoints();

var context = app.Services.GetRequiredService<UserProfileContext>();
await context.Database.MigrateAsync();
await app.RunAsync();
