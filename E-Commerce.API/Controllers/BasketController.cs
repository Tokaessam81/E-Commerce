using AutoMapper;
using E_Commerce.API.Controllers;
using E_Commerce.API.Error;
using Ecommerce.Core.Entities;
using Ecommerce.Core.ServiceContract;
using Ecommerce.Core.Services.Contract;
using Ecommerce.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketService basketService, IMapper mapper, ILogger<BasketController> logger)
        {
            _basketService = basketService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{basketId}")]
        [ProducesResponseType(typeof(CustomerBasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDTO>> GetBasket(string basketId)
        {
            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket == null)
            {
                return NotFound(new ApiResponse(400, "Basket not found" ));
            }

            return Ok(_mapper.Map<CustomerBasketDTO>(basket));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDTO>> UpdateBasket(CustomerBasketDTO basketDto)
        {
            var basketEntity = _mapper.Map<CustomerBasket>(basketDto);
            var updatedBasket = await _basketService.UpdateBasketAsync(basketEntity);

            if (updatedBasket == null)
            {
                return BadRequest(new ApiResponse(400, "Failed to update basket" ));
            }

            return Ok(_mapper.Map<CustomerBasketDTO>(updatedBasket));
        }

        [HttpDelete("{basketId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteBasket(string basketId)
        {
            var deleted = await _basketService.DeleteBasketAsync(basketId);
            if (!deleted)
            {
                return BadRequest(new ApiResponse(400, "Failed to delete basket"));
            }

            return NoContent();
        }
    }
}
