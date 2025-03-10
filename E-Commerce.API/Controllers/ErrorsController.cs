
using E_Commerce.API.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("error/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return code switch
            {
                StatusCodes.Status404NotFound => NotFound(new ApiResponse(code)),
                StatusCodes.Status401Unauthorized=>Unauthorized(new ApiResponse(code)),
                _=>StatusCode(code),
            };
        }
    }
}
