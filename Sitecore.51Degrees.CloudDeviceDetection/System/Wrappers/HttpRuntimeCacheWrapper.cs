using System.Web.Caching;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers
{
    public interface IHttpRuntimeCacheWrapper
    {
        void Set<T>(string cacheKey, T value);

        T Get<T>(string cacheKey);
    }

    public class HttpRuntimeCacheWrapper : IHttpRuntimeCacheWrapper
    {
        private readonly IHttpContextWrapper _httpContextWrapper;
        private readonly IHttpRuntimeWrapper _httpRuntimeWrapper;

        public HttpRuntimeCacheWrapper(IHttpContextWrapper httpContextWrapper, IHttpRuntimeWrapper httpRuntimeWrapper)
        {
            _httpContextWrapper = httpContextWrapper;
            _httpRuntimeWrapper = httpRuntimeWrapper;
        }

        public void Set<TType>(string cacheKey, TType value)
        {
            var httpContext = _httpContextWrapper.GetHttpContext();

            if (httpContext != null)
            {
                if (ReferenceEquals(value, null))
                {
                    _httpRuntimeWrapper.Cache.Remove(cacheKey);
                    return;
                }

                _httpRuntimeWrapper.Cache.Add(cacheKey, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
        }

        public TType Get<TType>(string cacheKey)
        {
            TType tType = default(TType);
            var httpContext = _httpContextWrapper.GetHttpContext();

            if (httpContext != null)
            {
                return (TType)_httpRuntimeWrapper.Cache[cacheKey];
            }

            return tType;
        }
    }
}
