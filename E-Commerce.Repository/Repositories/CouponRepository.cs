using E_Commerce.Repository.Data;
using Ecommerce.Core.Entities;
using Ecommerce.Core.RepositoriesContract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Repositories
{
    public class CouponRepository : GenericRepo<Coupon>, ICouponRepository
    {
        private readonly ECommerceDbContext _context;

        public CouponRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Coupon> GetByCodeAsync(string code)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == code);

            if (coupon == null)
                return
                    null!;
            return coupon;
        }
    }
}
