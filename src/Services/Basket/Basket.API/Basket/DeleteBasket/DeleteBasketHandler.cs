namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess);

    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(c => c.UserName)
                .NotEmpty()
                .WithMessage("User name is required.");
        }
    }

    public class DeleteBasketCommandHandler(IBasketRepository repository)
        : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            // TODO: delete basket from the database or cache
            var status = await repository.DeleteBasket(command.UserName, cancellationToken);

            return new DeleteBasketResult(status);
        }
    }
}
