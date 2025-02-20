using Ecommerce.Core.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Ecommerce.Core.Entities
{
    public class Order : BaseEntity
    {
        // Default Constructor for EF Core
        public Order() { }

        // Parameterized Constructor
        public Order(int Userid,string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod,
                     List<OrderItem> orderItems, decimal subTotal, string? paymentIntentId)
        {
            UserId = Userid;
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems ?? new List<OrderItem>();
            SubTotal = subTotal;
            PaymentInitId = paymentIntentId;
            OrderCreatedDate = DateTime.Now;
            Status = OrderStatus.Pending;
        }

        // Properties
        public string BuyerEmail { get; set; }
        public DateTime OrderCreatedDate { get; set; } = DateTime.Now;
        public decimal SubTotal { get; set; }
        public decimal DeliveryCost { get; set; }
        public OrderStatus Status { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int? AddressId { get; set; }
        public int? CouponId { get; set; }  

        // Navigation Properties
        public Coupon? Coupon { get; set; }
        public Address ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public AppUser User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public string? PaymentInitId { get; set; }

        // Computed Property
        public decimal GetTotal() => SubTotal + DeliveryCost;
    }
}
