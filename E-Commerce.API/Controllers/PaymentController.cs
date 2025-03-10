using AutoMapper;
using E_Commerce.API.Controllers;
using E_Commerce.API.Error;
using E_Commerce.API.Helpers;
using Ecommerce.Core.DTOS;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Services.Contract;
using Ecommerce.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TalabatAPI.Controllers
{
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IPaymentServices _paymentServices;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentServices paymentServices,
            IMapper mapper,
            ILogger<PaymentController> logger)
        {
            _paymentServices = paymentServices ?? throw new ArgumentNullException(nameof(paymentServices));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("{basketId}")]
        //[Cache(300)] // Cache the response for 5 minutes
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerBasketDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        public async Task<ActionResult<PaymentDTO>> CreateOrUpdatePayment(string basketId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(basketId))
                {
                    _logger.LogWarning("Basket ID is null or empty.");
                    return BadRequest(new ApiResponse(400, "Basket ID is required."));
                }

                var basket = await _paymentServices.CreatePaymentIntent(basketId);
                if (basket == null)
                {
                    _logger.LogWarning("Payment intent creation failed for basket ID: {BasketId}", basketId);
                    return BadRequest(new ApiResponse(400, "Unable to create payment intent."));
                }

               

                var payment = new PaymentDTO
                {
                    PaymentIntintId = basket.PaymentIntetId !
                };
              
                _logger.LogInformation("Payment intent created successfully for basket ID: {BasketId}", basketId);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating or updating payment for basket ID: {BasketId}", basketId);
                return StatusCode(500, new ApiResponse(500, "An internal server error occurred."));
            }
        }
    }
}