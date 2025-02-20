using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.RepositoriesContract
{
    public interface ICouponRepository : IGenericRepo<Coupon>
    {
        Task<Coupon> GetByCodeAsync(string code);
    }

}
