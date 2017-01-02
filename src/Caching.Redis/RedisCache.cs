/**********************************************************************************************************************
 * 描述：
 *      Redis 缓存。
 * 
 * 变更历史：
 *      作者：李亮  时间：2016年12月31日	 新建
 *********************************************************************************************************************/

using System;
using System.Linq;
using System.Threading;
using StackExchange.Redis;
using Wlitsoft.Framework.Common.Core;
using Wlitsoft.Framework.Common.Extension;
using Wlitsoft.Framework.Common.Exception;

namespace Wlitsoft.Framework.Caching.Redis
{
    /// <summary>
    /// Redis 缓存。
    /// </summary>
    public class RedisCache : IDistributedCache
    {
        #region 私有属性

        //redis 连接实例。
        private volatile ConnectionMultiplexer _connection;

        //redis 缓存数据库实例。
        private IDatabase _database;

        //连接实例锁。
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        //Redis 配置。
        internal static RedisCacheConfiguration RedisCacheConfiguration;

        #endregion

        #region IDistributedCache 成员

        /// <summary>
        /// 获取缓存。
        /// </summary>
        /// <typeparam name="T">缓存类型。</typeparam>
        /// <param name="key">缓存键值。</param>
        /// <returns>获取到的缓存。</returns>
        public T Get<T>(string key)
        {
            #region 参数校验

            if (string.IsNullOrEmpty(key))
                throw new StringNullOrEmptyException(nameof(key));

            #endregion

            this.Connect();
            string result = this._database.StringGet(key);
            if (string.IsNullOrEmpty(result))
                return default(T);
            return result.ToJsonObject<T>();
        }

        /// <summary>
        /// 设置缓存。
        /// </summary>
        /// <typeparam name="T">缓存类型。</typeparam>
        /// <param name="key">缓存键值。</param>
        /// <param name="value">要缓存的对象。</param>
        public void Set<T>(string key, T value)
        {
            #region 参数校验

            if (string.IsNullOrEmpty(key))
                throw new StringNullOrEmptyException(nameof(key));

            if (value == null)
                throw new ObjectNullException(nameof(value));

            #endregion

            this.Connect();
            this._database.StringSet(key, value.ToJsonString());
        }

        /// <summary>
        /// 设置缓存。
        /// </summary>
        /// <typeparam name="T">缓存类型。</typeparam>
        /// <param name="key">缓存键值。</param>
        /// <param name="value">要缓存的对象。</param>
        /// <param name="expiredTime">过期时间。</param>
        public void Set<T>(string key, T value, TimeSpan expiredTime)
        {
            #region 参数校验

            if (string.IsNullOrEmpty(key))
                throw new StringNullOrEmptyException(nameof(key));

            if (value == null)
                throw new ObjectNullException(nameof(value));

            #endregion

            this.Connect();
            this._database.StringSet(key, value.ToJsonString(), expiredTime);
        }

        /// <summary>
        /// 判断指定键值的缓存是否存在。
        /// </summary>
        /// <param name="key">缓存键值。</param>
        /// <returns>一个布尔值，表示缓存是否存在。</returns>
        public bool Exists(string key)
        {
            #region 参数校验

            if (string.IsNullOrEmpty(key))
                throw new StringNullOrEmptyException(nameof(key));

            #endregion

            this.Connect();
            return this._database.KeyExists(key);
        }

        /// <summary>
        /// 移除指定键值的缓存。
        /// </summary>
        /// <param name="key">缓存键值。</param>
        public bool Remove(string key)
        {
            #region 参数校验

            if (string.IsNullOrEmpty(key))
                throw new StringNullOrEmptyException(nameof(key));

            #endregion

            this.Connect();
            return this._database.KeyDelete(key);
        }

        /// <summary>
        /// 获取 Hash 表中的缓存。
        /// </summary>
        /// <typeparam name="T">缓存类型。</typeparam>
        /// <param name="key">缓存键值。</param>
        /// <param name="hashField">要获取的 hash 字段。</param>
        /// <returns>获取到的缓存。</returns>
        public T GetHash<T>(string key, string hashField)
        {
            #region 参数校验

            if (string.IsNullOrEmpty(key))
                throw new StringNullOrEmptyException(nameof(key));

            if (string.IsNullOrEmpty(hashField))
                throw new StringNullOrEmptyException(nameof(hashField));

            #endregion

            this.Connect();
            string value = this._database.HashGet(key, hashField);
            if (string.IsNullOrEmpty(value))
                return default(T);
            return value.ToJsonObject<T>();
        }

        /// <summary>
        /// 设置 缓存到 Hash 表。
        /// </summary>
        /// <typeparam name="T">缓存类型。</typeparam>
        /// <param name="key">缓存键值。</param>
        /// <param name="hashField">要设置的 hash 字段。</param>
        /// <param name="hashValue">要设置的 hash 值。</param>
        public void SetHash<T>(string key, string hashField, T hashValue)
        {
            #region 参数校验

            if (string.IsNullOrEmpty(key))
                throw new StringNullOrEmptyException(nameof(key));

            if (string.IsNullOrEmpty(hashField))
                throw new StringNullOrEmptyException(nameof(hashField));

            if (hashValue == null)
                throw new ObjectNullException(nameof(hashValue));

            #endregion

            this.Connect();
            this._database.HashSet(key, hashField, hashValue.ToJsonString());
        }

        /// <summary>
        /// 判断指定键值的 Hash 缓存是否存在。
        /// </summary>
        /// <param name="key">缓存键值。</param>
        /// <param name="hashField">hash 字段。</param>
        /// <returns>一个布尔值，表示缓存是否存在。</returns>
        public bool ExistsHash(string key, string hashField)
        {
            #region 参数校验

            if (string.IsNullOrEmpty(key))
                throw new StringNullOrEmptyException(nameof(key));

            if (string.IsNullOrEmpty(hashField))
                throw new StringNullOrEmptyException(nameof(hashField));

            #endregion

            this.Connect();
            return this._database.HashExists(key, hashField);
        }

        /// <summary>
        /// 删除 hash 表中的指定字段的缓存。
        /// </summary>
        /// <param name="key">缓存键值。</param>
        /// <param name="hashField">hash 字段。</param>
        /// <returns>一个布尔值，表示缓存是否删除成功。</returns>
        public bool DeleteHash(string key, string hashField)
        {
            #region 参数校验

            if (string.IsNullOrEmpty(key))
                throw new StringNullOrEmptyException(nameof(key));

            if (string.IsNullOrEmpty(hashField))
                throw new StringNullOrEmptyException(nameof(hashField));

            #endregion

            this.Connect();
            return this._database.HashDelete(key, hashField);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 连接。
        /// </summary>
        private void Connect()
        {
            if (this._connection != null)
                return;

            this._connectionLock.Wait();
            try
            {
                if (this._connection == null)
                {
                    this._connection = ConnectionMultiplexer.Connect(this.GetConfigurationOptions());
                    this._database = this._connection.GetDatabase();
                }
            }
            finally
            {
                this._connectionLock.Release();
            }
        }

        private ConfigurationOptions GetConfigurationOptions()
        {
            #region 校验

            if (RedisCacheConfiguration == null)
                throw new ObjectNullException(nameof(RedisCacheConfiguration));

            if (!RedisCacheConfiguration.HostAndPoints.Any())
                throw new Exception("RedisCahce 的 HostAndPoints 不能为空");

            #endregion

            ConfigurationOptions options = new ConfigurationOptions();

            foreach (string item in RedisCacheConfiguration.HostAndPoints)
                options.EndPoints.Add(item);

            options.ConnectRetry = RedisCacheConfiguration.ConnectRetry;
            options.ConnectTimeout = RedisCacheConfiguration.ConnectTimeout;

            return options;
        }

        #endregion

        #region 析构函数

        /// <summary>
        /// 析构 <see cref="RedisCache"/> 类型的对象。
        /// </summary>
        ~RedisCache()
        {
            _connection?.Close();
        }

        #endregion
    }
}
