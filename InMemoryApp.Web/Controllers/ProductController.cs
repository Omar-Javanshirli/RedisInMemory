using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Cryptography;
using System.Security.Policy;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            //if (string.IsNullOrEmpty(this.memoryCache.Get<string>("Zaman")))
            //    this.memoryCache.Set<string>("Zaman", DateTime.Now.ToString());

            this.memoryCache.Remove("Zaman");

            // Bu method-a Key gonderirik ve o yoxluyur ki elaqeli key Memoride var ya yox. Bool gaytarir.
            if (!this.memoryCache.TryGetValue("Zaman", out string zamanCache))
            {
                //Cache-deki datanin omrunu bildirmek
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
                //options.SlidingExpiration = TimeSpan.FromSeconds(10);
                options.Priority = CacheItemPriority.High;

                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    this.memoryCache.Set("callback", $"{key} => {value} => sebeb : {reason}");
                });

                this.memoryCache.Set<string>("Zaman", DateTime.Now.ToString(), options);
            }

            Product p = new Product() { Id = 1, Name = "Pencil", Price = 100 };
            this.memoryCache.Set<Product>("product:1", p);
            return View();
        }

        public IActionResult Show()
        {
            //Silme emeliyyati
            //this.memoryCache.Remove("Zaman");

            //Key Memoride varsa Deyerin gaytarir yoxdusa Memoride elaqeli key - i yaradir.
            //this.memoryCache.GetOrCreate<string>("zaman", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});

            this.memoryCache.TryGetValue("Zaman", out string zamanCache);
            this.memoryCache.TryGetValue("callback", out string callback);
            ViewBag.zaman = zamanCache;
            ViewBag.callback = callback;
            ViewBag.product = this.memoryCache.Get<Product>("product:1");
            //ViewBag.zaman = this.memoryCache.Get<string>("Zaman");
            return View();
        }
    }
}
