using HS.Message.Share.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Redis
{
    /// <summary>
    /// redis
    /// </summary>
    public class RedisCache : IDistributedCache, IDisposable
    {

        private ConnectionMultiplexer _redisConnection;
        private readonly object _redisConnectionLock = new();
        private readonly IConfiguration _configuration;
        private readonly ILogger<RedisCache> _logger;
        private IDatabase _db;
        private readonly int _defaultExpiryHours = 2;

        #region 分布式锁对象

        private RedLockFactory _redLockFactory;
        private IRedLock _redisLock;
        #endregion

        /// <summary>
        /// redis
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public RedisCache(IConfiguration configuration, ILogger<RedisCache> logger)
        {
            _configuration = configuration;
            _logger = logger;
            if (!int.TryParse(configuration["Redis:DefaultExpiryHours"], out _defaultExpiryHours))
            {
                _defaultExpiryHours = 2;
            }
        }
        private ConnectionMultiplexer GetConnection()
        {
            if (_redisConnection != null && _redisConnection.IsConnected)
            {
                return _redisConnection; // 已有连接，直接使用
            }
            lock (_redisConnectionLock)
            {
                if (_redisConnection != null && _redisConnection.IsConnected)
                {
                    return _redisConnection; // 已有连接，直接使用
                }
                else
                {
                    try
                    {
                        string connectString = $"{_configuration["Redis:EndPoints"]},password={_configuration["Redis:Password"]}";
                        _redisConnection = ConnectionMultiplexer.Connect(connectString);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Redis服务启动失败：{ex.Message}");
                        throw ex;
                    }
                }
            }
            return _redisConnection;
        }
        private IDatabase RedisDB
        {
            get
            {
                try
                {
                    GetConnection();
                    _db ??= _redisConnection.GetDatabase();
                    return _db;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Redis读取数据库失败：{ex.Message}");
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 创建key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string BuildKey(string key)
        {
            return $"{_configuration["Redis:Prefix"]}:{key}";
        }
        /// <summary>
        /// 设置值
        /// Set key to hold the string value. If key already holds a value, it is overwritten, regardless of its type.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCache(string key, object value)
        {
            SetCache(key, value, TimeSpan.FromHours(_defaultExpiryHours));
        }
        /// <summary>
        /// 设置值
        /// Set key to hold the string value. If key already holds a value, it is overwritten, regardless of its type.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetCacheAsync(string key, object value)
        {
            await SetCacheAsync(key, value, TimeSpan.FromHours(_defaultExpiryHours));
        }
        /// <summary>
        /// 设置值
        /// Set key to hold the string value. If key already holds a value, it is overwritten, regardless of its type.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">过期时间</param>
        public void SetCache(string key, object value, TimeSpan expiry)
        {
            string cacheKey = BuildKey(key);
            RedisDB.StringSet(cacheKey, value.ToJson(), expiry);
        }
        /// <summary>
        /// 设置值
        /// Set key to hold the string value. If key already holds a value, it is overwritten, regardless of its type.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task SetCacheAsync(string key, object value, TimeSpan expiry)
        {
            string cacheKey = BuildKey(key);
            await RedisDB.StringSetAsync(cacheKey, value.ToJson(), expiry);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetCache(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException($"redis key 不能为空！");
            }
            string cacheKey = BuildKey(key);
            if (RedisDB.KeyExists(cacheKey))
                return RedisDB.StringGet(cacheKey);
            else
                return null;
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetCacheAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException($"redis key 不能为空！");
            }
            string cacheKey = BuildKey(key);
            if (await RedisDB.KeyExistsAsync(cacheKey))
                return await RedisDB.StringGetAsync(cacheKey);
            else
                return null;
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetCache<T>(string key)
        {
            var cacheValue = GetCache(key);
            if (!string.IsNullOrWhiteSpace(cacheValue))
            {
                return cacheValue.ToObject<T>();
            }
            return default;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetCacheAsync<T>(string key)
        {
            var cacheValue = await GetCacheAsync(key);
            if (!string.IsNullOrWhiteSpace(cacheValue))
            {
                return cacheValue.ToObject<T>();
            }
            else
                return default;
        }
        /// <summary>
        /// 获取值
        /// 如果值不存在，则新增，如果值已存在，则返回值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<string> GetOrAddAsync(string key, object value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException($"redis key 不能为空！");
            }
            string cacheKey = BuildKey(key);
            string cacheValue;
            var cacheExists = await RedisDB.KeyExistsAsync(cacheKey);
            if (cacheExists)
                cacheValue = await GetCacheAsync(key);
            else
            {
                cacheValue = value.ToJson();
                if (!expiry.HasValue) expiry = TimeSpan.FromHours(_defaultExpiryHours);
                await RedisDB.StringSetAsync(cacheKey, cacheValue, expiry);
            }
            return cacheValue;
        }
        /// <summary>
        /// 获取值
        /// 如果值不存在，则新增，如果值已存在，则返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="valueFactory">缓存不存在时，值的获取方式</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFactory, TimeSpan? expiry = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException($"redis key 不能为空！");
            }
            string cacheKey = BuildKey(key);
            T cacheValue;
            var cacheExists = await RedisDB.KeyExistsAsync(cacheKey);
            if (cacheExists)
                cacheValue = await GetCacheAsync<T>(key);
            else
            {
                //从委托中取实际值，并写入缓存
                cacheValue = await valueFactory();
                if (cacheValue != null)
                {
                    if (!expiry.HasValue) expiry = TimeSpan.FromHours(_defaultExpiryHours);
                    await SetCacheAsync(key, cacheValue, expiry.Value);
                }
            }
            return cacheValue;
        }
        /// <summary>
        /// 获取值
        /// 如果值不存在，则新增，如果值已存在，则返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="valueFactory">缓存不存在时，值的获取方式</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<List<T>> GetOrAddListAsync<T>(string key, Func<Task<List<T>>> valueFactory, TimeSpan? expiry = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException($"redis key 不能为空！");
            }
            string cacheKey = BuildKey(key);
            List<T> cacheValue = null;
            var cacheExists = await RedisDB.KeyExistsAsync(cacheKey);
            if (cacheExists)
                cacheValue = await GetCacheAsync<List<T>>(key);
            else if (cacheValue == null || cacheValue.Count == 0)
            {
                //从委托中取实际值，并写入缓存
                cacheValue = await valueFactory();
                if (cacheValue?.Count > 0)
                {
                    if (!expiry.HasValue) expiry = TimeSpan.FromHours(_defaultExpiryHours);
                    await SetCacheAsync(key, cacheValue, expiry.Value);
                }
            }
            return cacheValue;
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveCache(string key)
        {
            return RedisDB.KeyDelete(BuildKey(key));
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RemoveCacheAsync(string key)
        {
            return await RedisDB.KeyDeleteAsync(BuildKey(key));
        }

        #region Redis分布式锁
        /// <summary>
        /// 加分布式锁,自动解锁
        /// </summary>
        /// <param name="action"></param>
        /// <param name="lockKey">锁key</param>
        /// <param name="lockExpiry">锁自动过期时间[默认10]（s）</param>
        /// <param name="waitLockSeconds">等待锁时间（ms）</param>
        /// <returns>LockId</returns>
        public string LockAndRelease(Action action, string lockKey, TimeSpan lockExpiry, long waitLockSeconds = 10)
        {
            if (string.IsNullOrWhiteSpace(lockKey))
            {
                throw new ArgumentNullException($"redis key 不能为空！");
            }

            lockKey = BuildKey($"DistributedLock:{lockKey}");

            var redisConnection = GetConnection();
            var multiplexers = new List<RedLockMultiplexer>() { redisConnection };
            using var redLockFactory = RedLockFactory.Create(multiplexers);
            using var redisLock = redLockFactory.CreateLock(lockKey, lockExpiry, TimeSpan.FromSeconds(waitLockSeconds), TimeSpan.FromSeconds(1));
            if (redisLock.IsAcquired)
            {
                action();
            }

            return redisLock.LockId;
        }

        /// <summary>
        /// 加分布式锁，必须调用ReleaseLock()手动解锁
        /// </summary>
        /// <param name="lockKey">锁key</param>
        /// <param name="lockExpiry">锁自动过期时间[默认10]（s）</param>
        /// <param name="waitLockSeconds">等待锁时间（ms）</param>
        /// <returns>LockId</returns>
        public bool TakeLock(string lockKey, TimeSpan lockExpiry, long waitLockSeconds = 10)
        {
            if (string.IsNullOrWhiteSpace(lockKey))
            {
                throw new ArgumentNullException($"redis key 不能为空！");
            }

            lockKey = BuildKey($"DistributedLock:{lockKey}");

            var redisConnection = GetConnection();
            var multiplexers = new List<RedLockMultiplexer>() { redisConnection };
            _redLockFactory = RedLockFactory.Create(multiplexers);
            _redisLock = _redLockFactory.CreateLock(lockKey, lockExpiry, TimeSpan.FromSeconds(waitLockSeconds), TimeSpan.FromSeconds(1));

            return _redisLock.IsAcquired;
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        public void ReleaseLock()
        {
            _redLockFactory?.Dispose();
            _redisLock?.Dispose();
        }

        #endregion

        /// <summary>
        /// Dispose redis connection
        /// </summary>
        public void Dispose()
        {
            if (_redisConnection != null)
            {
                ReleaseLock();
                if (_redisConnection.IsConnected)
                    _redisConnection.Close();
                _redisConnection.Dispose();
            }
        }

    }

}
