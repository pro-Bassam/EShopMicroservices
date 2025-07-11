﻿namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public async void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products",
            async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProductResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update product")
            .WithDescription("Update a product from the database.");
    }
}