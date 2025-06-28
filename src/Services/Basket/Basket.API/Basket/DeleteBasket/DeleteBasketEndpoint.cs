namespace Basket.API.Basket.DeleteBasket
{
    //public record DeleteBasketRequest(string UserName);
    public record DeleteBasketResponse(bool IsSuccess);

    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var command = new DeleteBasketCommand(userName);
                var response = await sender.Send(command);
                return Results.Ok(response);
            })
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>()
            .WithSummary("Delete a user's basket")
            .WithDescription("Deletes the basket for the specified user from the database or cache.");
        }
    }
}
