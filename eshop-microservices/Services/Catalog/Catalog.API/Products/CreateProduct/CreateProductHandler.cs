namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(
        string Name,
        List<string> Category,
        string Description,
        string ImageFile,
        decimal Price
    ) : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Product name is required.");

            RuleFor(c => c.Category)
                .NotEmpty()
                .WithMessage("Product category is required.");

            RuleFor(c => c.ImageFile)
                .NotEmpty()
                .WithMessage("ImageFile is required.");

            RuleFor(c => c.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");
        }
    }

    internal class CreateProductCommandHandler(
        IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(
            CreateProductCommand command,
            CancellationToken cancellationToken
        )
        {
            // Business logic to create a product
            // create a product entity from command object
            // save the product entity to the database
            // return the CreateProductResult result

            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            session.Store(product); // to save the product as a document database
            await session.SaveChangesAsync(cancellationToken);
            
            return new CreateProductResult(product.Id);
        }
    }
}
