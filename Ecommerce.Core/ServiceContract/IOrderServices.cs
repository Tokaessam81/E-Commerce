using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Core.DTOS;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Services.Contract
{
    public interface IOrderServices
    {
        Task<Order> CreateAsync(string BuyerEmail,string basketId, Address shippingAddress, int DeliveryMethodId, string? paymentIntentId);
        Task<IReadOnlyList<Order>> GetOrdersAsync(string BuyerEmail);
        Task<Order> GetOrderByIdAsync(int id ,string BuyerEmail);
        Task<bool> CancelOrderAsync(int id, string buyerEmail);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> ConfirmPayment(string paymentIntentId);
        Task<bool> CheckStockAvailability(string basketId);
        Task<bool> RestockItems(int orderId);
        Task SendOrderConfirmationEmail(string email, int orderId);

        //Task<Order?> UpdateOrderAsync(int orderId, string userEmail, int? deliveryMethodId, Address? shippingAddress,OrderStatus orderStatus);


    }
}
