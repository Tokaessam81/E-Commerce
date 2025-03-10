using Ecommerce.Core.ServiceContract;
using StackExchange.Redis;
using System.Text.Json;

namespace E_Commerce.Services
{
    public class ResponseCashedService : IResponseCachedService
    {
        private readonly IDatabase _db;
        public ResponseCashedService(IConnectionMultiplexer Redis)
        {
            try
            {
                _db = Redis.GetDatabase();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async Task<bool> RemoveAsync(string Key)
        {
            try
            {
                return await _db.KeyDeleteAsync(Key);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        public async Task<string?> GetCachedResponseAsync(string Key)
        {
            try
            {
                var response = await _db.StringGetAsync(Key);
                if (response.IsNullOrEmpty) return null;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;

            }
        }

        public async Task CacheResponseAsync(string Key, object? Value, TimeSpan ExpirationDate)
        {
            try
            {if (Value is null)
                return;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(Value, options);

                await _db.StringSetAsync(Key, json); }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
               
            }
        }
    }
}
