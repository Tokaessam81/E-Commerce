using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Services.Contract
{
    public interface IPaymentServices
    {
        Task<CustomerBasket?> CreatePaymentIntent(string basketId);
        Task<bool> ConfirmPayment(string paymentIntentId);

    }
}
