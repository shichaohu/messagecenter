using RedLockNet.SERedis.Configuration;
using RedLockNet.SERedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Message.Share.Redis
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface IDistributedCache
    {
        #region 设置缓存 
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">值</param>
        void SetCache(string key, object value);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">值</param>
        Task SetCacheAsync(string key, object value);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        void SetCache(string key, object value, TimeSpan expiry);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        Task SetCacheAsync(string key, object value, TimeSpan expiry);
        #endregion

        #region 获取缓存

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        string GetCache(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        Task<string> GetCacheAsync(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        T GetCache<T>(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        Task<T> GetCacheAsync<T>(string key);

        /// <summary>
        /// 获取缓存,没有则添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">实际值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        Task<string> GetOrAddAsync(string key, object value, TimeSpan? expiry = null);

        /// <summary>
        /// 获取泛型缓存,没有则添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="valueFactory">获取实际值的委托</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFactory, TimeSpan? expiry = null);

        /// <summary>
        /// 获取泛型集合缓存,没有则添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="valueFactory">获取实际值的委托</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        Task<List<T>> GetOrAddListAsync<T>(string key, Func<Task<List<T>>> valueFactory, TimeSpan? expiry = null);

        #endregion

        #region 分布式缓存
        /// <summary>
        /// 加分布式锁，并自动释放锁
        /// </summary>
        /// <param name="action"></param>
        /// <param name="lockKey">锁key</param>
        /// <param name="lockExpiry">锁自动过期时间[默认10]（s）</param>
        /// <param name="waitLockSeconds">等待锁时间（ms）</param>
        /// <returns>LockId</returns>
        string LockAndRelease(Action action, string lockKey, TimeSpan lockExpiry, long waitLockSeconds = 10);

        /// <summary>
        /// 加分布式锁
        /// 必须调用ReleaseLock()手动解锁
        /// </summary>
        /// <param name="lockKey">锁key</param>
        /// <param name="lockExpiry">锁自动过期时间[默认10]（s）</param>
        /// <param name="waitLockSeconds">等待锁时间（ms）</param>
        /// <returns>LockId</returns>
        bool TakeLock(string lockKey, TimeSpan lockExpiry, long waitLockSeconds = 10);

        /// <summary>
        /// 释放锁
        /// 使用TakeLock方法加锁后，才需调用此方法
        /// </summary>
        void ReleaseLock();
        #endregion

        #region 删除缓存

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        bool RemoveCache(string key);

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        Task<bool> RemoveCacheAsync(string key);

        #endregion

    }
}
