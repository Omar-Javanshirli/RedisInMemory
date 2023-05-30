using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualBasic;
using System;
using System.Threading.Tasks;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {

        private readonly IDistributedCache distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            this.distributedCache.SetString("name", "Leyla", cacheOptions);
            return View();
        }

        public  IActionResult Show()
        {
            var name =  this.distributedCache.GetString("name");
            ViewBag.Name = name;
            return View();
        }

        public IActionResult Remove()
        {
            this.distributedCache.Remove("name");
            return View();
        }
    }
}
