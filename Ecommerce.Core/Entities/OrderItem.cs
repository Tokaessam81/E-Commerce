using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class OrderItem: BaseEntity
    {
        public int Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public float? Discount { get; set; }
        public string? Color { get; set; }
        //Foreign Key
        public int? ProductId { get; set; }

        public int OrderId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
