using Ecommerce.Core.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.ServiceContract
{
    public interface ICouponService
    {
        Task<ApplyCouponResult> ApplyCouponAsync(string code, decimal totalPrice);
        Task<CouponDto> CreateCouponAsync(CouponDto model);
        Task<List<CouponDto>> GetAllCouponsAsync();
    }
}
