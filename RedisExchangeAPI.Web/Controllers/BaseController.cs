using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Servicies;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisService redisService;
        protected readonly IDatabase db;

        public BaseController(RedisService redisService)
        {
            this.redisService = redisService;
            this.db = this.redisService.GetDb(1);
        }
    }
}
