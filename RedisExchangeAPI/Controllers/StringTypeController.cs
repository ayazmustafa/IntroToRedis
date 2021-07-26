using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            db.StringSet("name", "Mustafa Ayaz");
            db.StringSet("visitor", 100);
            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name");
            db.StringIncrement("visitor", 1);
            db.StringDecrementAsync("visitor", 10).Wait();
            if(value.HasValue)
            {
                ViewBag.value = value.ToString();
            }
            return View();
        }
    }
}
