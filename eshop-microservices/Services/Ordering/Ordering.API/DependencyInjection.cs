namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Register application services here
            // Example: services.AddScoped<IOrderService, OrderService>();

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            // Configure application services here
            // Example: app.UseMiddleware<CustomMiddleware>();
            // app.MapCarter()

            return app;
        }
    }
}
