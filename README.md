# Wlitsoft.Framework.Caching
Wlitsoft 框架 - 分布式缓存

## Redis 实现

Caching.Redis 使用第三方组件 StackExchange.Redis 实现 Redis 访问客户端。

### 相关配置

> 设置 Redis 缓存配置

1. 对象硬编码设置

   `App.Builder.SetRedisCacheConfig(new RedisCacheConfiguration() { … });`

2. 以项目配置文件 AppSettings 节点设置 Redis 缓存

   `App.Builder.SetRedisCacheConfigByAppSettings();`

   ​