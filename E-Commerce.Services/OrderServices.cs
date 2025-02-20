

using Ecommerce.Core;
using Ecommerce.Core.DTOS;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.RepositoriesContract;
using Ecommerce.Core.Repository.Contract;
using Ecommerce.Core.ServiceContract;
using Ecommerce.Core.Services.Contract;
using Ecommerce.Core.Specifications.OrderSpecific;
using Stripe;
using Address = Ecommerce.Core.Entities.Address;
using Product = Ecommerce.Core.Entities.Product;

namespace Ecommerce.Services
{
    public class OrderServices : IOrderServices
    {
        
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitofWork _unitOfWork;
        private readonly IPaymentServices _payment;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public OrderServices(IBasketRepository basketRepo
           , IUnitofWork unitOfWork,IPaymentServices payment,IEmailService emailService, IUserRepository userRepository)
        {
            _basketRepo = basketRepo;
           _unitOfWork = unitOfWork;
            _payment = payment;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        public async Task<Order> CreateAsync(string BuyerEmail, string basketId, Address shippingAddress, int DeliveryMethodId, string? paymentIntentId)
        {
            if (string.IsNullOrWhiteSpace(basketId))
                throw new ArgumentException("Basket ID is required.");

            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket?.Items == null || basket.Items.Count == 0)
                throw new Exception("Basket is empty or not found.");

            var user = await _userRepository.GetUserByEmailAsync(BuyerEmail);
            if (user == null)
                throw new Exception("User not found.");

            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product not found with ID: {item.ProductId}");

                if (product.Quantity < item.Quantity)
                    throw new Exception($"Insufficient stock for product: {product.Name}");

                product.Quantity -= item.Quantity;

                var pictureUrl = product.Images?.FirstOrDefault()?.ImageUrl ?? "default-image-url";
                var orderItem = new OrderItem(item.Color, item.ProductId, product.Name, pictureUrl, product.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            var subTotal = orderItems.Sum(o => o.Price * o.Quantity);
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            if (deliveryMethod == null)
                throw new Exception("Delivery method not found.");

            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                var paymentConfirmed = await _payment.ConfirmPayment(paymentIntentId);
                if (!paymentConfirmed)
                    throw new Exception("Payment confirmation failed.");
            }

            var order = new Order(user.Id, BuyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, paymentIntentId);

            await _unitOfWork.Repository<Order>().AddAsync(order);

            foreach (var item in orderItems)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    _unitOfWork.Repository<Product>().Update(product);
                }
            }

            var result = await _unitOfWork.completeAsync();
            if (result <= 0)
                throw new Exception("Failed to save order to database.");

            try
            {
                await SendOrderConfirmationEmail(BuyerEmail, order.Id);
            }
            catch (Exception ex)
            {
                // يمكن تسجيل الخطأ هنا إذا لزم الأمر
            }

            return order;
        }


        public async Task SendOrderConfirmationEmail(string email, int orderId)
        {
            var subject = "Your Order Confirmation";
            var message = $"Your order with ID {orderId} has been successfully placed.";
            await _emailService.SendEmailAsync(email, subject, message);
        }
        public async Task<Order> GetOrderByIdAsync(int id, string BuyerEmail)
        {
            var repo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecification(id, BuyerEmail);
            var order = await repo.GetByIdSpecification(spec);
            return order;

        }

        public async Task<IReadOnlyList<Order>> GetOrdersAsync(string BuyerEmail)
        {
            var repo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecification(BuyerEmail);
            var order = await repo.GetAllSpecification(spec);
            return order;
        }
    
        public async Task<bool> CancelOrderAsync(int id, string buyerEmail)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (order == null || order.BuyerEmail != buyerEmail)
                return false;

            if (order.Status != OrderStatus.Pending)
                return false;

            // 🛑 إعادة المنتجات إلى المخزون عند الإلغاء
            foreach (var item in order.OrderItems)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                    _unitOfWork.Repository<Product>().Update(product);
                }
            }

            order.Status = OrderStatus.Cancelled;
            _unitOfWork.Repository<Order>().Update(order);

            return await _unitOfWork.completeAsync() > 0;
        }
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            _unitOfWork.Repository<Order>().Update(order);
            return await _unitOfWork.completeAsync() > 0;
        }

        public async Task<bool> ConfirmPayment(string paymentIntentId)
        {
            var paymentService = new PaymentIntentService();
            var paymentIntent = await paymentService.GetAsync(paymentIntentId);
            if (paymentIntent != null)
            {
                paymentIntent.Status = "succeeded";
                return true;
            }
            return false;
        }

        public async Task<bool> CheckStockAvailability(string basketId)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket == null) return false;

            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                if (product.Quantity < item.Quantity) return false;
            }
            return true;
        }

        public async Task<bool> RestockItems(int orderId)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order == null) return false;

            foreach (var item in order.OrderItems)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                    _unitOfWork.Repository<Product>().Update(product);
                }
            }
            return await _unitOfWork.completeAsync() > 0;
        }


    }
}
