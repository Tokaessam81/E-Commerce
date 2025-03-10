using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.DTOS
{
    public class ApplyCouponRequest
    {
        public string Code { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
