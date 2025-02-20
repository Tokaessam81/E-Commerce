using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.DTOS
{
  
        public class OrderDto
        {
            public int Id { get; set; }
            public string Status { get; set; }
            public DateTime OrderDate { get; set; }
            public List<OrderItemDto> OrderItems { get; set; }
        }

    
}
