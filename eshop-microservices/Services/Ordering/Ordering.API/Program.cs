using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container DI.
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

// Build the app
var app = builder.Build();

// Configure the HTTP request middleware pipeline
app.UseApiServices();

app.Run();
