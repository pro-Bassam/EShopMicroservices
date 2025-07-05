using Discount.Grpc;

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

    public class StoreBasketCommandHandler
        (IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command.Cart, cancellationToken);

            var cart = await repository.StoreBasket(command.Cart, cancellationToken);

            return new StoreBasketResult(cart.UserName);
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken1)
        {
            // Communicate with Discount.Grpc and calculate latest prices of products
            foreach (var item in cart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(
                    new GetDiscountRequest { ProductName = item.ProductName },
                    cancellationToken: cancellationToken1);
                if (coupon is { Amount: > 0 })
                    item.Price -= coupon.Amount;
            }
        }
    }
}
