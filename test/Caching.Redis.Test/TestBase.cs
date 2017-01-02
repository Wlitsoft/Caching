using Wlitsoft.Framework.Common;
using Wlitsoft.Framework.Caching.Redis;

namespace Caching.Redis.Test
{
    public class TestBase
    {
        static TestBase()
        {
            App.Builder.SetRedisCacheConfigByAppSettings();
        }
    }
}
