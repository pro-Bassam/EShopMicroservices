﻿using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter("Npgsql", LogLevel.Warning); // suppress verbose Npgsql info logs

// Add Services to the container e.g. DI
builder.Services.AddCarter();

var assembly = Assembly.GetExecutingAssembly(); // NOTE: It is the same as typeof(Program).Assembly;

// MediatR + Pipeline Behaviors
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);
    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// FluentValidation: register all validators in this assembly
builder.Services.AddValidatorsFromAssembly(assembly);

// Marten (PostgreSQL as document store)
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

// Centralized Exception Handling
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Health Checks + PostgreSQL
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

// Build the app
var app = builder.Build();

// Middleware pipeline
// Global Exception Middleware
app.UseExceptionHandler(options => { });

// Health Check endpoint (returns UI-compatible JSON)
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

// NOTE: MediatR pipeline behaviors are not part of the ASP .NET Core HTTP middleware pipeline
// Configure the HTTP request pipeline
app.MapCarter();

app.Run();