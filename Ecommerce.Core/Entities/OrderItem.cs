namespace Ecommerce.Core.Entities
{
    public class OrderItem : BaseEntity
    {
        // Default Constructor for EF Core
        public OrderItem() { }

        // Constructor with required properties
        public OrderItem(string?color ,int productId, string productName, string pictureUrl, decimal price, int quantity)
        {
            Color = color;
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
            Quantity = quantity;
        }

        // Properties
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public float? Discount { get; set; }
        public string? Color { get; set; }

        // Foreign Keys & Navigation Properties
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
