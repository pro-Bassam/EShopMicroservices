using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;
public static class Extensions
{
    // Adds an extension method `UseMigration` to `IApplicationBuilder`
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        // Create a new DI scope to safely retrieve scoped services
        using var scope = app.ApplicationServices.CreateScope();

        // Retrieve the DiscountContext from DI within this scope
        using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();

        // Apply pending migrations asynchronously
        dbContext.Database.MigrateAsync();

        // Return the app so you can chain it in your Program.cs
        return app;
    }
}