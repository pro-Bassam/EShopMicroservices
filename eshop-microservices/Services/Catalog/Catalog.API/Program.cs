using BuildingBlocks.Behaviors;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter("Npgsql", LogLevel.Warning); // suppress verbose Npgsql info logs

// Add Services to DI
builder.Services.AddCarter();

var assembly = Assembly.GetExecutingAssembly(); // NOTE: It is the same as typeof(Program).Assembly;

// MediatR + Pipeline Behaviors
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);
    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
});

// FluentValidation: register all validators in this assembly
builder.Services.AddValidatorsFromAssembly(assembly);

// Marten (PostgreSQL as document store)
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
})
    .UseLightweightSessions();

// Build the app
var app = builder.Build();

// Middleware pipeline
// NOTE: MediatR pipeline behaviors are not part of the ASP .NET Core HTTP middleware pipeline
// Configure the HTTP request pipeline
app.MapCarter();

app.Run();