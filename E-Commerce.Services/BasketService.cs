//using Ecommerce.Core;
//using Ecommerce.Core.DTOS;
//using Ecommerce.Core.Entities;
//using Ecommerce.Core.Repository.Contract;
//using Ecommerce.Core.ServiceContract;
//using Ecommerce.Core.Services.Contract;
//using Ecommerce.DTO;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Ecommerce.Services
//{
//    public class BasketService : IBasketService
//    {
//        private readonly IBasketRepository _basketRepository;
//        private readonly IUnitofWork _unitOfWork;

//        public BasketService(IBasketRepository basketRepository, IUnitofWork unitOfWork)
//        {
//            _basketRepository = basketRepository;
//            _unitOfWork = unitOfWork;
//        }

//        private async Task<CustomerBasket> EnsureBasketExists(string basketId)
//        {
//            return await _basketRepository.GetBasketAsync(basketId) ?? new CustomerBasket { Id = basketId, Items = new List<BasketItem>() };
//        }

//        public async Task<CustomerBasketDTO> GetBasketAsync(string basketId)
//        {
//            var basket = await _basketRepository.GetBasketAsync(basketId);
//            if (basket == null) return null!;

//            return new CustomerBasketDTO
//            {
//                Id = basket.Id,
//                Items = basket.Items.Select(item => new BasketItemDTO
//                {
//                    ProductId = item.ProductId,
//                    ProductName = item.Name,
//                    ImageUrl = item.PictureUrl, 
//                    Price = item.Price,
//                    Quantity = item.Quantity
//                }).ToList()
//            };
//        }

//        public async Task<CustomerBasketDTO> AddToBasketAsync(string basketId, string productId, int quantity)
//        {
//            var basket = await EnsureBasketExists(basketId);

//            if (!int.TryParse(productId, out int productIdInt))
//            {
//                throw new Exception("Invalid Product ID format");
//            }

//            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productIdInt);
//            if (product == null) throw new Exception("Product not found");

//            var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == productId);
//            if (existingItem == null)
//            {
//                basket.Items.Add(new BasketItem
//                {
//                    ProductId = productId, // يبقى string لأن السلة تُخزن في Redis
//                    Name = product.Name,
//                    PictureUrl = product.Images.FirstOrDefault()?.ImageUrl ?? "",
//                    Quantity = quantity,
//                    Price = product.Price
//                });
//            }
//            else
//            {
//                existingItem.Quantity += quantity;
//            }

//            await _basketRepository.UpdateBasketAsync(basket);
//            return await GetBasketAsync(basketId);
//        }


//        public async Task<CustomerBasketDTO> RemoveFromBasketAsync(string basketId, string productId)
//        {
//            var basket = await _basketRepository.GetBasketAsync(basketId);
//            if (basket == null) return null!;

//            var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
//            if (item == null) return await GetBasketAsync(basketId);

//            basket.Items.Remove(item);
//            await _basketRepository.UpdateBasketAsync(basket);
//            return await GetBasketAsync(basketId);
//        }

//        public async Task<CustomerBasketDTO> UpdateQuantityAsync(string basketId, string productId, int quantity)
//        {
//            if (quantity <= 0) return await RemoveFromBasketAsync(basketId, productId);

//            var basket = await _basketRepository.GetBasketAsync(basketId);
//            if (basket == null) return null!;

//            var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
//            if (item == null) return await GetBasketAsync(basketId);

//            item.Quantity = quantity;
//            await _basketRepository.UpdateBasketAsync(basket);
//            return await GetBasketAsync(basketId);
//        }

//        public async Task<bool> ClearBasketAsync(string basketId)
//        {
//            return await _basketRepository.DeleteBasketAsync(basketId);
//        }
//    }
//}
using Ecommerce.Core;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repository.Contract;
using Ecommerce.Core.ServiceContract;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitofWork _unitofWork; 

    public BasketService(IBasketRepository basketRepository, IUnitofWork unitofWork)
    {
        _basketRepository = basketRepository;
        _unitofWork = unitofWork;
    }

    public async Task<CustomerBasket?> GetBasketAsync(string basketId)
    {
        return await _basketRepository.GetBasketAsync(basketId);
    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
    {
        if (basket.Items.Any())
        {
            foreach (var item in basket.Items)
            {
                var product = await _unitofWork.Repository<Product>().GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    item.Price = product.Price; 
                }
            }
        }

        return await _basketRepository.UpdateBasketAsync(basket);
    }

    public async Task<bool> DeleteBasketAsync(string basketId)
    {
        return await _basketRepository.DeleteBasketAsync(basketId);
    }

    public async Task ClearBasketAsync(string basketId)
    {
        await _basketRepository.DeleteBasketAsync(basketId);
    }
}
