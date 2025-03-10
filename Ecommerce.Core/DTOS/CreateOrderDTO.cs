using Ecommerce.Core.DTOS;
using Ecommerce.Core.Entities;

public class CreateOrderDto
{
    public string BasketId { get; set; } = string.Empty;
    public AddressDTO ShippingAddress { get; set; }
    public string Color { get; set; }
    public int DeliveryMethodId { get; set; }
    public string? PaymentIntentId { get; set; }
}