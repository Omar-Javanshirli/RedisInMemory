using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Servicies;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            this.redisService = redisService;
            this.db = this.redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            db.StringSet("name", "Omer Cavanshirli");
            db.StringSet("visitor", 100);
            db.StringSet("audience", 200);

            return View();
        }

        public async Task<IActionResult> Show()
        {
            var value = this.db.StringGet("name");
            var value2= this.db.StringIncrement("visitor",1);
            var audience = await this.db.StringDecrementAsync("audience", 10);

            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
                ViewBag.visitor = value2.ToString();
                ViewBag.audience= audience.ToString();
            }

            return View();
        }
    }
}
