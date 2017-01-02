/**********************************************************************************************************************
 * 描述：
 *      应用 构造 静态扩展类。
 * 
 * 变更历史：
 *      作者：李亮  时间：2016年12月31日	 新建
 * 
 *********************************************************************************************************************/

using System;
using System.Configuration;
using System.Linq;
using Wlitsoft.Framework.Common;
using Wlitsoft.Framework.Common.Exception;

namespace Wlitsoft.Framework.Caching.Redis
{
    /// <summary>
    /// 应用 构造 静态扩展类。
    /// </summary>
    public static class AppBuilderExtension
    {
        /// <summary>
        /// 设置 Redis 缓存配置。
        /// </summary>
        /// <param name="appBuilder">应用构造。</param>
        /// <param name="config">Redis 缓存配置。</param>
        public static void SetRedisCacheConfig(this AppBuilder appBuilder, RedisCacheConfiguration config)
        {
            #region 参数校验

            if (config == null)
                throw new ObjectNullException(nameof(config));

            #endregion

            RedisCache.RedisCacheConfiguration = config;
        }

        /// <summary>
        /// 以项目配置文件 AppSettings 节点设置 Redis 缓存。
        /// </summary>
        /// <param name="appBuilder"></param>
        public static void SetRedisCacheConfigByAppSettings(this AppBuilder appBuilder)
        {
            RedisCacheConfiguration config = new RedisCacheConfiguration();
            if (ConfigurationManager.AppSettings["RedisCache.HostAndPoints"] != null)
            {
                config.HostAndPoints = ConfigurationManager.AppSettings["RedisCache.HostAndPoints"].Split(';').ToList();
            }
            if (ConfigurationManager.AppSettings["RedisCache.ConnectRetry"] != null)
                config.ConnectRetry = Convert.ToInt32(ConfigurationManager.AppSettings["RedisCache.ConnectRetry"]);

            if (ConfigurationManager.AppSettings["RedisCache.ConnectTimeout"] != null)
                config.ConnectTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["RedisCache.ConnectTimeout"]);

            appBuilder.SetRedisCacheConfig(config);
        }
    }
}
