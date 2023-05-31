using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Servicies;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;
        private string listKey = "cities";

        public SetTypeController(RedisService redisService)
        {
            this.redisService = redisService;
            this.db = this.redisService.GetDb(2);
        }

        public IActionResult Index()
        {
            HashSet<string> cities = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    cities.Add(x.ToString());
                });
            }

            return View(cities);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name)
        {
            if (!db.KeyExists(listKey))
            {
                //listkey-ye 5 deyqe omur veririk. 
                db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
            }

            await db.SetAddAsync(listKey, name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(string name)
        {
            await db.SetRemoveAsync(listKey,name);
            return RedirectToAction(nameof(Index));
        }
    }
}
