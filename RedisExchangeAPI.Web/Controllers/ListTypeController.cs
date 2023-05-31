using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Servicies;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;
        private string listkey = "names";

        public ListTypeController(RedisService redisService)
        {
            this.redisService = redisService;
            this.db = this.redisService.GetDb(1);
        }

        public IActionResult Index()
        {
            LinkedList<string> nameList = new LinkedList<string>();

            if (this.db.KeyExists(listkey))
            {
                this.db.ListRange(listkey).ToList().ForEach(x =>
                {
                    nameList.AddLast(x.ToString());
                });
            }

            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            this.db.ListRightPush(listkey, name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(string name)
        {
            await this.db.ListRemoveAsync(listkey, name);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem()
        {
            db.ListLeftPop(listkey);
            return RedirectToAction("Index");
        }
    }
}
