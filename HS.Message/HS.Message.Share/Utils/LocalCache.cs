using Microsoft.Extensions.Caching.Memory;

namespace HS.Message.Share.Utils
{
    /// <summary>
    /// 本地缓存帮助类
    /// </summary>
    public class LocalCache
    {
        //本地内存缓存
        private IMemoryCache _meoryCache;
        public LocalCache(IMemoryCache memoryCache)
        {
            _meoryCache = memoryCache;
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool SetValue<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (expiry == null)
            {
                _meoryCache.Set<T>(key, value);
            }
            else
            {
                _meoryCache.Set<T>(key, value, (TimeSpan)expiry);
            }
            return true;
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            if (_meoryCache.TryGetValue<T>(key, out T result))
            {
                return result;
            }
            else
                return default(T);
        }
        public void RemoveKey(string key)
        {
            _meoryCache.Remove(key);
        }
    }
}