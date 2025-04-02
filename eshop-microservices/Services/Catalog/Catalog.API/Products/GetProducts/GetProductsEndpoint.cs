namespace Catalog.API.Products.GetProducts;

//public record GetProductsRequest();
public record GetProductResponse(IEnumerable<Product> Products);
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var query = new GetProductsQuery();
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductResponse>()
        .WithSummary("Get all products")
        .WithDescription("Retrieves all products from the database.");
    }
}