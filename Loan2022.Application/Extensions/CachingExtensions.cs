using System.Text.Json;
using System.Text.Json.Serialization;
using AspNetCoreHero.ThrowR;
using Microsoft.Extensions.Caching.Distributed;

namespace Loan2022.Application.Extensions
{
    public static class CachingExtensions
{
    public static async Task<T> GetAsync<T>(
            this IDistributedCache distributedCache,
            string cacheKey,
            CancellationToken token = default (CancellationToken))
        {
            Throw.Exception.IfNull<IDistributedCache>(distributedCache, nameof (distributedCache));
            Throw.Exception.IfNull<string>(cacheKey, nameof (cacheKey));
            byte[] utf8Bytes = await distributedCache.GetAsync(cacheKey, token).ConfigureAwait(false);
            T async = utf8Bytes == null ? default (T) : JsonSerializer.Deserialize<T>((ReadOnlySpan<byte>) utf8Bytes);
            utf8Bytes = (byte[]) null;
            return async;
        }

        public static async Task RemoveAsync(
            this IDistributedCache distributedCache,
            string cacheKey,
            CancellationToken token = default (CancellationToken))
        {
            Throw.Exception.IfNull<IDistributedCache>(distributedCache, nameof (distributedCache));
            Throw.Exception.IfNull<string>(cacheKey, nameof (cacheKey));
            await distributedCache.RemoveAsync(cacheKey, token).ConfigureAwait(false);
        }

        public static async Task SetAsync<T>(
            this IDistributedCache distributedCache,
            string cacheKey,
            T obj,
            int cacheExpirationInMinutes = 30,
            CancellationToken token = default (CancellationToken))
        {
            Throw.Exception.IfNull<IDistributedCache>(distributedCache, nameof (distributedCache));
            Throw.Exception.IfNull<string>(cacheKey, nameof (cacheKey));
            Throw.Exception.IfNull<T>(obj, nameof (obj));
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes((double) cacheExpirationInMinutes));
            byte[] utf8Bytes = JsonSerializer.SerializeToUtf8Bytes<T>(obj, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            });
            await distributedCache.SetAsync(cacheKey, utf8Bytes, options, token).ConfigureAwait(false);
            options = (DistributedCacheEntryOptions) null;
            utf8Bytes = (byte[]) null;
        }
    }
}