﻿namespace Catalog.API.Products.GetProductByCategory
{
    //public record GetProductByCategoryRequest();
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}",
                async (string category, ISender sender) =>
                {
                    var query = new GetProductByCategoryQuery(category);
                    var result = await sender.Send(query);
                    var response = result.Adapt<GetProductByCategoryResponse>();
                    return Results.Ok(response);
                })
                .WithName("GetProductByCategory")
                .Produces<GetProductByCategoryResponse>()
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get product by category")
                .WithDescription("Retrieves products from the database.");
        }
    }
}
