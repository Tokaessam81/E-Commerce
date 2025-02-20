using Ecommerce.Core.Common.Constants;
using Ecommerce.Core.DTOS;
using Ecommerce.Core.ServiceContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Authorize(Roles =AuthorizationConstants.CustomerRole)]
    public class CouponsController : BaseController
    {
        
        private readonly ICouponService _couponService;

        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpPost("apply")]

        public async Task<ActionResult> ApplyCoupon([FromBody] ApplyCouponRequest request)
        {
            var result = await _couponService.ApplyCouponAsync(request.Code, request.TotalPrice);
            return result.Success ? Ok(result) : BadRequest(new { message = result.Message });
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateCoupon([FromBody] CouponDto model)
        {
            var createdCoupon = await _couponService.CreateCouponAsync(model);
            return Ok(createdCoupon);
        }

        [HttpGet("all")]

        public async Task<ActionResult> GetAllCoupons()
        {
            var coupons = await _couponService.GetAllCouponsAsync();
            return Ok(coupons);
        }
    }
    }
