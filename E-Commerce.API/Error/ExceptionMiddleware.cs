
using E_Commerce.API.Error;
using System.Net;
using System.Text.Json;

namespace Ecommerce.API.Errors
{
    public class ExceptionMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _Next;
        private readonly IWebHostEnvironment _Env;

        public ExceptionMiddleware(RequestDelegate Next,ILogger<ExceptionMiddleware> logger,IWebHostEnvironment Env)
        {
            _Env = Env;
            _Next = Next;   
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _Next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                httpContext.Response.ContentType= "application/json";
                httpContext.Response.StatusCode=(int) HttpStatusCode.InternalServerError;


                var response =
                   _Env.IsDevelopment() ?
                   new ApiExtensionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) :
                   new ApiExtensionResponse((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var json=JsonSerializer.Serialize(response, options);
               await httpContext.Response.WriteAsync(json);
            }


        }
    }
}
