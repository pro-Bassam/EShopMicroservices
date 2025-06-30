namespace Basket.API.Basket.GetBasket;

//public record GetBasketRequest(string UserName);

public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}",
        async (string userName, ISender sender) =>
        {
            var query = new GetBasketQuery(userName);
            var result = await sender.Send(query);
            var response = result.Adapt<GetBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("GetBasketByUserName")
        .Produces<GetBasketResponse>()
        .WithSummary("Get Basket by UserName")
        .WithDescription("Retrieves the shopping cart for a specific user by their username.");
    }
}