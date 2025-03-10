using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public float? Discount { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? Color { get; set; }
        public float? Rating { get; set; }
        public ProductStatus status { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
          public List<ProductImage> Images { get; set; } = new List<ProductImage>();

    }
}
