using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.DTOS
{
    public class CouponDto
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public decimal DiscountAmount { get; set; }
        public double DiscountPercentage { get; set; }
        public int MaxUsage { get; set; }
        public int UsedCount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }
}
