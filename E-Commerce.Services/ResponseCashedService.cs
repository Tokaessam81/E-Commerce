using Ecommerce.Core.ServiceContract;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using IDatabase = StackExchange.Redis.IDatabase;

namespace E_Commerce.Services
{
    public class ResponseCashedService : IResponseCachedService
    {
        private readonly IDatabase _db;
        public ResponseCashedService(IConnectionMultiplexer Redis)
        {
            _db = Redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string CacheKey, object Response, TimeSpan ExpireDate)
        {
            if(Response == null)
            {
                return ;
            }
            var option = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var ResponseSerelizer = JsonSerializer.Serialize(Response, option);
            await _db.StringSetAsync(CacheKey, ResponseSerelizer, ExpireDate);

        }
        public async Task<string?> GetCachedResponseAsync(string CacheKey)
        {
            var response = await _db.StringGetAsync(CacheKey);
            if (response.IsNullOrEmpty) return null;
            return response;
        }
        public async Task RemoveAsync(string cacheKey)
        {
            await _db.KeyDeleteAsync(cacheKey);
        }
    }
}
