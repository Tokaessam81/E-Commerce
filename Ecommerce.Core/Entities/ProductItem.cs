namespace Ecommerce.Core.Entities
{
    public class ProductItem
    {
        private object pictureUrl;

        public ProductItem(int id, string name, List<string> pictureUrl)
        {
            Id = id;
            Name = name;
            this.pictureUrl = pictureUrl;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public float? Discount { get; set; }
        public List<string>? ImagesUrl { get; set; } = new List<string>();
        public string? ShortDescription { get; set; }
        public string? Color { get; set; }
        public float? Rating { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}