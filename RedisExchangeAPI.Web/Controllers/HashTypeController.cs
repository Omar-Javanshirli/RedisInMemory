using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Servicies;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        public string HashKey { get; set; } = "words";
        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string,string>list= new Dictionary<string,string>();
            if (db.KeyExists(HashKey))
            {
                db.HashGetAll(HashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }
            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            db.HashSet(HashKey, name, value);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(string name)
        {
            db.HashDelete(HashKey,name);
            return RedirectToAction(nameof(Index));
        }
    }
}
