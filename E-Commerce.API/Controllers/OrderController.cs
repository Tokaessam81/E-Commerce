using AutoMapper;
using E_Commerce.API.Controllers;
using E_Commerce.API.Error;
using Ecommerce.Core.Common.Constants;
using Ecommerce.Core.DTOS;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
 
    public class OrdersController : BaseController
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;

        public OrdersController(IOrderServices orderServices, IMapper mapper)
        {
            _orderServices = orderServices;
            _mapper = mapper;
        }

        private string? GetUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        [HttpGet]
        [Authorize(Roles = AuthorizationConstants.CustomerRole)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetOrders()
        {
            var email = GetUserEmail();
            if (email == null)
                return Unauthorized(new ApiResponse(401, "User email not found."));

            var orders = await _orderServices.GetOrdersAsync(email);
            return orders.Any() ? Ok(orders) : NotFound(new ApiResponse(400, "No orders found."));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = AuthorizationConstants.CustomerRole)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetOrderById(int id)
        {
            var email = GetUserEmail();
            if (email == null) return Unauthorized(new ApiResponse(401, "User email not found."));

            var order = await _orderServices.GetOrderByIdAsync(id, email);
            return order != null ? Ok(order) : NotFound(new ApiResponse(404, $"Order with ID {id} not found." ));
        }

        [HttpPost]
        [Authorize(Roles = AuthorizationConstants.CustomerRole)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid order data."));

            var email = GetUserEmail();
            if (email == null) return Unauthorized(new ApiResponse(401, "User email not found."));

            var shippingAddress = _mapper.Map<Address>(orderDto.ShippingAddress);

            if (!await _orderServices.CheckStockAvailability(orderDto.BasketId))
                return BadRequest(new ApiResponse(400, "Some products are out of stock."));

            if (!await _orderServices.ConfirmPayment(orderDto.PaymentIntentId))
                return BadRequest(new ApiResponse(400, "Payment confirmation failed."));

            var order = await _orderServices.CreateAsync(email, orderDto.BasketId, shippingAddress, orderDto.DeliveryMethodId, orderDto.PaymentIntentId);
            if (order == null) return BadRequest(new ApiResponse(400, "Order creation failed."));

            var orderDtoResponse = _mapper.Map<OrderDto>(order); // تحويل الـ Entity إلى DTO
            await _orderServices.SendOrderConfirmationEmail(email, order.Id);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, orderDtoResponse);
        }


        [HttpPut("{id}/status")]
        [Authorize(Roles = AuthorizationConstants.AdminRole)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdateDto statusUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid status update data."));

            var updated = await _orderServices.UpdateOrderStatusAsync(id, statusUpdateDto.Status);
            return updated ? Ok(new ApiResponse(200, "Order status updated successfully." ))
                           : BadRequest(new ApiResponse(400, "Failed to update order status."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AuthorizationConstants.CustomerRole)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var email = GetUserEmail();
            if (email == null) return Unauthorized(new ApiResponse(401, "User email not found.")); ;

            var result = await _orderServices.CancelOrderAsync(id, email);
            if (!result) return BadRequest(new ApiResponse(400, $"Failed to cancel order with ID {id}." ));
            await _orderServices.RestockItems(id);
            return NoContent();
        }
    }
}
