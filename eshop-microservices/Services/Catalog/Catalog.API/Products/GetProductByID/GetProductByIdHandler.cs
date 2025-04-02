namespace Catalog.API.Products.GetProductByID;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdQueryResult>;
public record GetProductByIdQueryResult(Product product);

public class GetProductByIdQueryHandler
    (IDocumentSession session, ILogger<GetProductByIdEndpoint> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResult>
{
    public async Task<GetProductByIdQueryResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation($"GetProductByIdQueryHandler.Handle called with {query}");

        var product = await session.Query<Product>()
            .Where(q => q.Id == query.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException();
        }
        return new GetProductByIdQueryResult(product);
    }

}