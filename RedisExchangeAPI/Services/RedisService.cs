using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Services
{
    public class RedisService
    {
        private ConnectionMultiplexer _redis;
        private readonly string _redisHost;
        private readonly string _redisPort;
        public IDatabase db { get; set; }
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }

        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";
            _redis = ConnectionMultiplexer.Connect(configString);

        }

        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
