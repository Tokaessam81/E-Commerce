
namespace Ecommerce.Core.Entities
{
    public class BasketItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public string Department { get; set; }
        public string? Color { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }

    }
}