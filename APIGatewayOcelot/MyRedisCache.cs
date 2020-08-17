using Ocelot.Cache;
using System;

namespace APIGatewayOcelot
{
    public class MyRedisCache : IOcelotCache<CachedResponse>
    {
        public void Add(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            if (!RedisHelper.Exists($"{region}_{key}"))
            {
                RedisHelper.Set($"{region}_{key}", value, 3600);
            }
        }

        public CachedResponse Get(string key, string region)
        {
            if (!RedisHelper.Exists($"{region}_{key}")) return null;

            var cacheObj = RedisHelper.Get<CachedResponse>($"{region}_{key}");
            if (cacheObj != null)
            {
                return cacheObj;
            }

            RedisHelper.Del($"{region}_{key}");

            return null;
        }

        public void ClearRegion(string region)
        {
            var keys = RedisHelper.Keys(region);

            foreach (var key in keys)
            {
                RedisHelper.Del(key);
            }
        }

        public void AddAndDelete(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            if (RedisHelper.Exists($"{region}_{key}"))
            {
                RedisHelper.Del($"{region}_{key}");
            }

            RedisHelper.Set($"{region}_{key}", value, 3600);
        }
    }
}