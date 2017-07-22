# Wlitsoft.Framework.Caching
Wlitsoft 框架 - 分布式缓存
[![Build status](https://ci.appveyor.com/api/projects/status/v575krel0vjv6ew5?svg=true)](https://ci.appveyor.com/project/Wlitsoft/caching)

## NuGet

包名称  | 当前版本 |
-------- | :------------ |
Wlitsoft.Framework.Caching.Redis | [![NuGet](https://img.shields.io/nuget/v/Wlitsoft.Framework.Caching.Redis.svg)](https://www.nuget.org/packages/Wlitsoft.Framework.Caching.Redis)

## Redis 实现

Caching.Redis 使用第三方组件 StackExchange.Redis 实现 Redis 访问客户端。

### 相关配置

> 设置 Redis 缓存配置

1. 对象硬编码设置

   `App.Builder.SetRedisCacheConfig(new RedisCacheConfiguration() { … });`

2. 以项目配置文件 AppSettings 节点设置 Redis 缓存

   `App.Builder.SetRedisCacheConfigByAppSettings();`

   ​