using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class Coupon:BaseEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } // كود الكوبون
        public string Type { get; set; } // نوع الكوبون (مثلاً: "شحن مجاني"، "خصم نسبة")
        public decimal DiscountAmount { get; set; } // قيمة الخصم الثابتة
        public double DiscountPercentage { get; set; } // نسبة الخصم
        public int MaxUsage { get; set; } // عدد المستخدمين المسموح به
        public int UsedCount { get; set; } = 0; // عدد المستخدمين الحالي
        public DateTime ExpiryDate { get; set; } // تاريخ انتهاء الصلاحية
        public bool IsActive { get; set; } = true; // هل الكوبون مفعل

    }

}
