using Ecommerce.Core.Entities;
using Ecommerce.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.OrderSpecific
{
    public class OrderWithPaymentIntentSpec : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpec(string PaymentintentId) :
           base(o => o.PaymentInitId == PaymentintentId)
        { 
        }
        }
}
