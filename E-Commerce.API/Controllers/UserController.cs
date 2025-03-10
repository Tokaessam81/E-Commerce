

using E_Commerce.API.Error;
using E_Commerce.API.Helpers;
using Ecommerce.Core.Common.Constants;
using Ecommerce.Core.DTOS;
using Ecommerce.Core.ServiceContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _user;

        public UserController(IUserService user)
        {
            _user = user;
        }
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(typeof(UserOperationsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [Authorize(Roles =AuthorizationConstants.AdminRole)]
        [Cache(30)]
        public async Task<ActionResult> GetUsers([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _user.GetUsersAsync(search, page, pageSize);
            if(result == null) 
                return NotFound(new ApiResponse(404,"No Users Found!"));
            return Ok(result);
        }
        [Authorize(Roles =AuthorizationConstants.AdminRole)]
        [HttpDelete("{Id}")]
        public async Task<ActionResult> RemoveUserAsync(string Id)
        {
             await _user.RemoveUserAsync(Id); 
            return NoContent();

        }
    }
}
