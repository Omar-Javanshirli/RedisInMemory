using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Servicies;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;
        private string listkey = "Animals";

        public SortedSetTypeController(RedisService redisService)
        {
            this.redisService = redisService;
            this.db = this.redisService.GetDb(3);
        }

        public IActionResult Index()
        {
            HashSet<string> animals = new HashSet<string>();
            if (this.db.KeyExists(listkey))
            {
                this.db.SortedSetScan(listkey).ToList().ForEach(x =>
                {
                    animals.Add(x.ToString());
                });
            }

            return View(animals);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name, int score)
        {
            await this.db.KeyExpireAsync(listkey, DateTime.Now.AddMinutes(1));
            await this.db.SortedSetAddAsync(listkey, name, score);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(string name)
        {
            await this.db.SortedSetRemoveAsync(listkey, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
