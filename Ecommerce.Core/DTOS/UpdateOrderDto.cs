using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.DTOS
{
    public class UpdateOrderDto
    {
        public int? DeliveryMethodId { get; set; }
        public AddressDTO? ShippingAddress { get; set; }
        public OrderStatus OrderStatus { get; set; } 
    }

}
