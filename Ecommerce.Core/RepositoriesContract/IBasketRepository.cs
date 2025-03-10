using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Repository.Contract
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket?> GetBasketAsync(string BasketId);
        public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
        public Task<bool> DeleteBasketAsync(string BasketId);
    }
}
