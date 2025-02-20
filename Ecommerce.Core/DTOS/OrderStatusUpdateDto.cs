using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.DTOS
{
    public class OrderStatusUpdateDto
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}
