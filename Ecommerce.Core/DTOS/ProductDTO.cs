
namespace Ecommerce.Core.DTOS
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public float? Discount { get; set; }
        public List<string> ImagesUrl { get; set; } = new List<string>();
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? Color { get; set; }
        public int? Rating { get; set; }
        public string Status { get; set; }
        public int? CategoryId { get; set; }
    }
}
