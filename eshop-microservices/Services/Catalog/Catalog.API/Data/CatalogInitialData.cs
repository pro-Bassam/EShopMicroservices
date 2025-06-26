using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellation))
            return;

        session.Store<Product>(GetPreConfiguredProducts());
        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreConfiguredProducts() => new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sample Product 1",
                Description = "This is a sample product description.",
                ImageFile = "product-1.png",
                Price = 19.99m,
                Category = new List<string> { "Category1" },
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sample Product 2",
                Description = "This is another sample product description.",
                ImageFile = "product-1.png",
                Price = 29.99m,
                Category = new List<string> { "Category2" },
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sample Product 3",
                Description = "This is yet another sample product description.",
                ImageFile = "product-1.png",
                Price = 39.99m,
                Category = new List<string> { "Category3" },
            }
        };
}