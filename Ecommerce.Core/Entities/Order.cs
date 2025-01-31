using Ecommerce.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class Order : BaseEntity
    {
        public string FirstName { get; set; }
        public DateTime OrderCreatedDate { get; set; }= DateTime.Now;
        public decimal subTotal { get; set; }
        public decimal DeliveryCost { get; set; }
        public OrderStatus Status { get; set; }
        //Foreign Key
        public int UserId { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }
      
        public AppUser User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public decimal GetTotal() => subTotal + DeliveryCost;


    }
}
