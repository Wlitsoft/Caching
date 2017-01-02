using System;
using System.Threading;
using Caching.Redis.Test.Fake;
using Wlitsoft.Framework.Caching.Redis;
using Xunit;

namespace Caching.Redis.Test
{
    public class RedisCacheTest : TestBase
    {

        private RedisCache _cache;

        public RedisCacheTest()
        {
            this._cache = new RedisCache();
        }

        [Fact]
        public void ConnectTest()
        {

        }

        [Fact]
        public void SetTest_String()
        {
            this._cache.Set<string>("test1", "hello");
        }

        [Fact]
        public void SetTest_String02()
        {
            //设置 test2 缓存 3秒。
            this._cache.Set<string>("test2", "hello,2017", TimeSpan.FromSeconds(3));

            //读取缓存。
            string result = this._cache.Get<string>("test2");

            Assert.Equal<string>("hello,2017", result);

            //睡 5 秒，判断缓存是否还存在。
            Thread.Sleep(5000);

            bool isExistes = this._cache.Exists("test2");

            Assert.True(!isExistes);
        }

        [Fact]
        public void GetTest_String()
        {
            string result = this._cache.Get<string>("test1");

            Assert.Equal("hello", result);
        }

        [Fact]
        public void SetTest_CustomObject()
        {
            PersonModel model = new PersonModel()
            {
                Name = "二饼",
                Age = 18
            };
            this._cache.Set<PersonModel>("p1", model);
        }

        [Fact]
        public void GetTest_CustomObject()
        {
            PersonModel result = this._cache.Get<PersonModel>("p1");

            Assert.NotNull(result);
            Assert.Equal<string>("二饼", result.Name);
            Assert.Equal<int>(18, result.Age);
        }

        [Fact]
        public void SetHashTest_String()
        {
            this._cache.SetHash<string>("h1", "f1", "fv1");
            this._cache.SetHash<string>("h1", "f2", "fv2");
        }

        [Fact]
        public void GetHashTest_String()
        {
            string result = this._cache.GetHash<string>("h1", "f2");

            Assert.Equal<string>("fv2", result);
        }

        [Fact]
        public void ExistsHashTest()
        {
            bool result = this._cache.ExistsHash("h1", "f1");

            Assert.True(result);
        }

        [Fact]
        public void DeleteHashTest()
        {
            this._cache.SetHash<string>("h1", "fd1", "xxx");
            bool result = this._cache.DeleteHash("h1", "fd1");

            Assert.True(result);
        }

        [Fact]
        public void ExistsTest()
        {
            bool result = this._cache.Exists("h1");

            Assert.True(result);
        }

        [Fact]
        public void RemoveTest()
        {
            bool result = this._cache.Remove("test1");

            Assert.True(result);

            string value = this._cache.Get<string>("test1");

            Assert.Null(value);
        }
    }
}
