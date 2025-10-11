namespace Ordering.Application.Extensions;

public static class OrderExtensions
{
    public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
    {
        return orders.Select(o => new OrderDto(
            Id: o.Id.Value,
            CustomerId: o.CustomerId.Value,
            OrderName: o.OrderName.Value,
            ShippingAddress: new AddressDto(
                o.ShippingAddress.FirstName,
                o.ShippingAddress.LastName,
                o.ShippingAddress.EmailAddress,
                o.ShippingAddress.AddressLine,
                o.ShippingAddress.Country,
                o.ShippingAddress.State,
                o.ShippingAddress.ZipCode
            ),
            BillingAddress: new AddressDto(
                o.BillingAddress.FirstName,
                o.BillingAddress.LastName,
                o.BillingAddress.EmailAddress,
                o.BillingAddress.AddressLine,
                o.BillingAddress.Country,
                o.BillingAddress.State,
                o.BillingAddress.ZipCode
            ),
            Payment: new PaymentDto(
                o.Payment.CardName,
                o.Payment.CardNumber,
                o.Payment.Expiration,
                o.Payment.CVV,
                o.Payment.PaymentMethod
            ),
            Status: o.Status,
            OrderItems: o.OrderItems.Select(oi => new OrderItemDto(
                oi.OrderId.Value,
                oi.ProductId.Value,
                oi.Quantity,
                oi.Price
            )).ToList()
        ));
    }
}
