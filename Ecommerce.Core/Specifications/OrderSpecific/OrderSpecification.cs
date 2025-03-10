using Ecommerce.Core.Entities;
using Ecommerce.Core.Specifications;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.OrderSpecific
{
    public class OrderSpecification:BaseSpecification<Order>
    {
        public OrderSpecification(string BuyerEmail):
            base(o=>o.User.Email==BuyerEmail)
        {
            Includes.Add(o=>o.ShippingAddress);
            Includes.Add(o=>o.OrderItems);
            AddOrderBy(o => o.OrderCreatedDate);
        }  
        public OrderSpecification(int Id,string BuyerEmail):
            base(o=>o.Id==Id && o.User.Email == BuyerEmail)
        {
            Includes.Add(o=>o.ShippingAddress);
            Includes.Add(o=>o.OrderItems);
           
        }
    }
}
