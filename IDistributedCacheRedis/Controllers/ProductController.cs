using IDistributedCacheRedis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDistributedCacheRedis.Controllers
{
    public class ProductController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;

        }

        public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            _distributedCache.SetString("name", "mustafa", cacheEntryOptions);
            return View();
        }

        public IActionResult Show()
        {
            string name = _distributedCache.GetString("name");
            ViewBag.name = name;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }

        public async Task<IActionResult> Index2()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            Product product = new Product { Id = 1, Name = "Pencil", Price = 100 };
            string jsonProduct = JsonConvert.SerializeObject(product);
            await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions); // product file//product:1-2-3-4-5
            return View();
        }

        public IActionResult Show2()
        {
            string jsonProduct = _distributedCache.GetString("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.product = p;
            return View();
        }

        public async Task<IActionResult> Index3()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(20);
            Product product = new Product { Id = 2, Name = "Pencil2", Price = 200 };
            string jsonProduct = JsonConvert.SerializeObject(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            _distributedCache.Set("product:2", byteProduct);
            return View();
        }

        public IActionResult Show3()
        {
            Byte[] byteProduct = _distributedCache.Get("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);
            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.product = p;
            return View();
        }
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/XD.jpg");
            byte[] imgByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("image", imgByte);
            return View();
        }
        public IActionResult ShowImage()
        {
            byte[] imgByte = _distributedCache.Get("image");
            return File(imgByte, "image/png");
        }


    }
}
