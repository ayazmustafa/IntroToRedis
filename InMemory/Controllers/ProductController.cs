using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using InMemory.Models;

namespace InMemory.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        public IActionResult Index()
        {

            //if(_memoryCache.TryGetValue("time", out string timecache))
            //{
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            options.SlidingExpiration = TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.High;
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key} --> {value} => reason: {reason}");
            });
            _memoryCache.Set<string>("time", DateTime.Now.ToString(), options);

            //}

            Product p = new Product { Id = 1, Name = "Nick", Price = 200 };
            _memoryCache.Set<Product>("Product:1", p);

            return View();
        }
        
        public IActionResult Show()
        {
            _memoryCache.TryGetValue("time", out string timecache);
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.time = timecache;
            ViewBag.callback = callback;
            ViewBag.product = _memoryCache.Get<Product>("Product:1");

            return View();
        }
    }
}