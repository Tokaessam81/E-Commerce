using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.ServiceContract
{
    public interface IResponseCachedService
    {
        Task CacheResponseAsync(string Key, object? Value, TimeSpan ExpirationDate);
        Task<string?> GetCachedResponseAsync(string cacheKey);
        Task<bool> RemoveAsync(string Key);
    }
}
