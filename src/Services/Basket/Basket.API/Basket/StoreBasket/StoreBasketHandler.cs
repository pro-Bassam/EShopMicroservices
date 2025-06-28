namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(c => c.Cart).NotNull().WithMessage("Cart cannot be null.");
            RuleFor(c => c.Cart.UserName).NotNull().WithMessage("");
        }
    }

    public class StoreBasketCommandHandler(IBasketRepository repository)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            // Todo: Store the shopping cart in the database
            // Todo: update cash
            var cart = await repository.StoreBasket(command.Cart, cancellationToken);

            return new StoreBasketResult(cart.UserName);
        }
    }
}
