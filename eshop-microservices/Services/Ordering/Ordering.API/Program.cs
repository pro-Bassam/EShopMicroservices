var builder = WebApplication.CreateBuilder(args);

// Add services to the container DI.

// Build the app
var app = builder.Build();

// Configure the HTTP request middleware pipeline

app.Run();
