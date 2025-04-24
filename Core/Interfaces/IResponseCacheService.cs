using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IResponseCacheService
    {
        public Task CacheResponseAsync(string email, string code, TimeSpan timeToLive);
        public Task<string> GetCachedResponse(string email);
        public Task DeleteCachedResponse(string email);
    }
}
