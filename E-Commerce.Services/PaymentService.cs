using Ecommerce.Core.Services.Contract;
using Microsoft.Extensions.Configuration;
using Stripe;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repository.Contract;
using Ecommerce.Core;
using Product = Ecommerce.Core.Entities.Product;

namespace Ecommerce.Services
{
    public class PaymentService : IPaymentServices
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitofWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitofWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket?> CreatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;

            foreach (var item in basket.Items)
            {
                
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                    if (product != null && item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                
             
            }

            var shippingCost = 0M;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingCost = deliveryMethod?.Cost ?? 0M;
            }

            var subTotal = basket.Items.Sum(item => item.Price * item.Quantity);
            var paymentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntetId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)((shippingCost + subTotal) * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await paymentService.CreateAsync(options);
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)((shippingCost + subTotal) * 100)
                };
                paymentIntent = await paymentService.UpdateAsync(basket.PaymentIntetId, options);
            }

            basket.PaymentIntetId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;
            await _basketRepository.UpdateBasketAsync(basket);
            return basket;
        }
        public async Task<bool> ConfirmPayment(string paymentIntentId)
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(paymentIntentId);
            if (paymentIntent != null)
            {
                paymentIntent.Status = "succeeded";
                return true;
            }
            return false;
        }

    }
}