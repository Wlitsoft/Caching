/**********************************************************************************************************************
 * 描述：
 *      Redis 缓存配置。
 * 
 * 变更历史：
 *      作者：李亮  时间：2016年12月31日	 新建
 *********************************************************************************************************************/

using System.Collections.Generic;
using System.Net;

namespace Wlitsoft.Framework.Caching.Redis
{
    /// <summary>
    /// Redis 缓存配置。
    /// </summary>
    public class RedisCacheConfiguration
    {
        /// <summary>
        /// 获取或设置 主机和端口号。
        /// </summary>
        public List<string> HostAndPoints { get; set; }

        /// <summary>
        /// 获取或设置 连接重试次数（默认 5 次）。
        /// </summary>
        public int ConnectRetry { get; set; }

        /// <summary>
        /// 获取或设置 连接超时时间单位毫秒（默认 5000 毫秒）。
        /// </summary>
        public int ConnectTimeout { get; set; }

        /// <summary>
        /// 初始化 <see cref="RedisCacheConfiguration"/> 类的新实例。
        /// </summary>
        public RedisCacheConfiguration()
        {
            this.HostAndPoints = new List<string>();
            this.ConnectRetry = 5;
            this.ConnectTimeout = 5000;
        }
    }
}
