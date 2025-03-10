using Ecommerce.Core.DTOS;
using Ecommerce.Core.ServiceContract;
using Ecommerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Core.Entities;
using Ecommerce.Core.RepositoriesContract;

namespace E_Commerce.Services
{
    public class CouponService : ICouponService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly ICouponRepository _couponRepo;

        public CouponService(IUnitofWork unitOfWork, ICouponRepository couponRepo)
        {
            _unitOfWork = unitOfWork;
            _couponRepo = couponRepo;
        }

            public async Task<ApplyCouponResult> ApplyCouponAsync(string code, decimal totalPrice)
            {
                var coupon = await _couponRepo.GetByCodeAsync(code);

                if (coupon == null || !coupon.IsActive || coupon.ExpiryDate < DateTime.UtcNow)
                    return new ApplyCouponResult { Success = false, Message = "Invalid or expired coupon." };

                if (coupon.MaxUsage > 0 && coupon.UsedCount >= coupon.MaxUsage)
                    return new ApplyCouponResult { Success = false, Message = "Coupon usage limit exceeded." };

                decimal discount = (coupon.DiscountAmount > 0 ? coupon.DiscountAmount : totalPrice * ((decimal)coupon.DiscountPercentage / 100));
                decimal newTotalPrice = Math.Max(0, totalPrice - discount);

                coupon.UsedCount++;
                await _unitOfWork.completeAsync();

                return new ApplyCouponResult { Success = true, DiscountAmount = discount, NewTotalPrice = newTotalPrice };
            }

            public async Task<CouponDto> CreateCouponAsync(CouponDto model)
            {
                var coupon = new Coupon
                {
                    Code = model.Code,
                    Type = model.Type,
                    DiscountAmount = model.DiscountAmount,
                    DiscountPercentage = model.DiscountPercentage,
                    MaxUsage = model.MaxUsage,
                    ExpiryDate = model.ExpiryDate,
                    IsActive = model.IsActive
                };

                await _unitOfWork.Repository<Coupon>().AddAsync(coupon);
                await _unitOfWork.completeAsync();
                return model;
            }

            public async Task<List<CouponDto>> GetAllCouponsAsync()
            {
                var coupons = await _unitOfWork.Repository<Coupon>().GetAllAsync();
                return coupons.Select(c => new CouponDto
                {
                    Code = c.Code,
                    Type = c.Type,
                    DiscountAmount = c.DiscountAmount,
                    DiscountPercentage = c.DiscountPercentage,
                    MaxUsage = c.MaxUsage,
                    UsedCount = c.UsedCount,
                    ExpiryDate = c.ExpiryDate,
                    IsActive = c.IsActive
                }).ToList();
            }
    }

    }
