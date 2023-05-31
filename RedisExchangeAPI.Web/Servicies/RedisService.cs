using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Servicies
{
    public class RedisService
    {
        private readonly IConfiguration configuration;
        private readonly string redisHost;
        private readonly string redisPort;

        //Bu class vasitesi ile Redis ile elaqe qurulur.
        private ConnectionMultiplexer redis;
        public IDatabase Db { get; set; }


        public RedisService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.redisHost = this.configuration["Redis:Host"];
            this.redisPort = this.configuration["Redis:Port"];
        }

        public void Connect()
        {
            var configString = $"{this.redisHost}:{this.redisPort}";
            this.redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDb(int db)
        {
            return this.redis.GetDatabase(db);
        }


     
    }
}
