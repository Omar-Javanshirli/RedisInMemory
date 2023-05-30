using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
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

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            await this.distributedCache.SetStringAsync("name", "Leyla", cacheOptions)!;
            return View();
        }

        public async Task<IActionResult> JsonProductSerialize()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            Product product = new Product { Id = 1, Name = "Pencil", Price = 10 };
            string jsonProduct = JsonConvert.SerializeObject(product);
            await this.distributedCache.SetStringAsync("product:1", jsonProduct, cacheOptions)!;
            return View();
        }

        public async Task<IActionResult> ShowJsonProductSerialize()
        {
            string jsonProduct = await this.distributedCache.GetStringAsync("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.Product = p;
            return View();
        }

        public async Task<IActionResult> ProductSerializeByBinary()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            Product product = new Product { Id = 1, Name = "Pencil", Price = 10 };
            string jsonProduct = JsonConvert.SerializeObject(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            await this.distributedCache.SetAsync("product:1", byteProduct)!;

            return View();
        }

        public async Task<IActionResult> ShowProductSerializeByBinariy()
        {
            Byte[] byteProduct = await this.distributedCache.GetAsync("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);

            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.Product = p;
            return View();
        }

        public async Task<IActionResult> Show()
        {
            var name = await this.distributedCache.GetStringAsync("name");
            ViewBag.Name = name;
            return View();
        }

        public async Task<IActionResult> Remove()
        {
            await this.distributedCache.RemoveAsync("name");
            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/omer.png");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            this.distributedCache.Set("resim",imageByte);

            return View();
        }

        public IActionResult ImageUri()
        {
            Byte[] imageByte = this.distributedCache.Get("resim");
            return File(imageByte, "image/png");
        }
    }
}
