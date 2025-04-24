using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;


namespace Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {

            database = redis.GetDatabase();
            
        }
        public async Task CacheResponseAsync(string email,string code,TimeSpan timeToLive)
        {          
            //var options = new JsonSerializerOptions {
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            //var cachedResponse = JsonSerializer.Serialize(code, options);
            await database.StringSetAsync(email, code, timeToLive);
            //await database.SetAddAsync(email,code);
        }

        public async Task DeleteCachedResponse(string email)
        {
            await database.KeyDeleteAsync(email);
        }

        public async Task<string> GetCachedResponse(string email)
        {
            var response=await database.StringGetAsync(email);
            if (response.IsNullOrEmpty)
                return null;
            return response;
        }
       
    }
}
