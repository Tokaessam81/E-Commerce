using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTO
{
    public class BasketItemDTO
    {
        public int ProductId { get; set; }
      
        public string ProductName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Color { get; set; }
        public int Quantity { get; set; }
    }
}