using Ecommerce.Core.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.API.Helpers
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
      
            private readonly int _expireTimeForSecond;

        public CacheAttribute(int ExpireTimeForSecond)
        {
            _expireTimeForSecond = ExpireTimeForSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {   
            var CacheServices = context.HttpContext.RequestServices.GetRequiredService<IResponseCachedService>();
            var CacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var CacheResponse = await CacheServices.GetCachedResponseAsync(CacheKey);
            if (CacheResponse != null)
            {
                var contentResult = new ContentResult()
                {
                    Content = CacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var ExecuteEndpointContext = await next.Invoke();
            if (ExecuteEndpointContext.Result is OkObjectResult result)
            {
                await CacheServices.CacheResponseAsync(CacheKey, result.Value, TimeSpan.FromSeconds(_expireTimeForSecond));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(request.Path);

            // إضافة الـ Query Parameters إلى المفتاح لضمان اختلاف الكاش بناءً على المعايير
            foreach (var (Key, Value) in request.Query.OrderBy(k => k.Key))
            {
                KeyBuilder.Append($"|{Key}-{Value}");
            }

            // إضافة UserId إلى المفتاح إذا كان المستخدم مسجّل دخول
            var userId = request.HttpContext.User.Identity.IsAuthenticated
                ? request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous"
                : "Anonymous";

            KeyBuilder.Append($"|User-{userId}");

            return KeyBuilder.ToString();
        }

    }
}

