namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public class GetOrdersByNameHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.OrderName.Value.Contains(query.OrderName))
            .OrderBy(o => o.OrderName)
            .ToListAsync(cancellationToken);

        var orderDtos = ProjectToOrderDtos(orders);

        return new GetOrdersByNameResult(orderDtos);
    }

    private static List<OrderDto> ProjectToOrderDtos(List<Order> orders)
    {
        var orderDtos = orders.Select(o => new OrderDto(
            Id: o.Id.Value,
            CustomerId: o.CustomerId.Value,
            OrderName: o.OrderName.Value,
            ShippingAddress: new AddressDto(
                FirstName: o.ShippingAddress.FirstName,
                LastName: o.ShippingAddress.LastName,
                EmailAddress: o.ShippingAddress.EmailAddress,
                AddressLine: o.ShippingAddress.AddressLine,
                Country: o.ShippingAddress.Country,
                State: o.ShippingAddress.State,
                ZipCode: o.ShippingAddress.ZipCode
            ),
            BillingAddress: new AddressDto(
                FirstName: o.BillingAddress.FirstName,
                LastName: o.BillingAddress.LastName,
                EmailAddress: o.BillingAddress.EmailAddress,
                AddressLine: o.BillingAddress.AddressLine,
                Country: o.BillingAddress.Country,
                State: o.BillingAddress.State,
                ZipCode: o.BillingAddress.ZipCode
            ),
            Payment: new PaymentDto(
                CardName: o.Payment.CardName,
                CardNumber: o.Payment.CardNumber,
                Expiration: o.Payment.Expiration,
                Cvv: o.Payment.CVV,
                PaymentMethod: o.Payment.PaymentMethod
            ),
            Status: o.Status,
            OrderItems: o.OrderItems.Select(oi => new OrderItemDto(
                OrderId: oi.OrderId.Value,
                ProductId: oi.ProductId.Value,
                Quantity: oi.Quantity,
                Price: oi.Price
            )).ToList()
        )).ToList();
        return orderDtos;
    }
}