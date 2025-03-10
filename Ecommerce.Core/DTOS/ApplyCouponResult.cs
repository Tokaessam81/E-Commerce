using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.DTOS
{
    public class ApplyCouponResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NewTotalPrice { get; set; }
    
}
}
