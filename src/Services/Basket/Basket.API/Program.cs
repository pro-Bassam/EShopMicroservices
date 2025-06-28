var builder = WebApplication.CreateBuilder(args);

// Add Services to the container e.g. DI
builder.Services.AddCarter();
var assembly = Assembly.GetExecutingAssembly(); // NOTE: It is the same as typeof(Program).Assembly;

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);
    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app.Run();
