using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Middlewares;

namespace UrlShortener.Controllers
{
    [MiddlewareFilter(typeof(CultureMiddleware))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(controllerName: "ShortUrls", actionName: "Index");
        }

        public IActionResult History()
        {
            return RedirectToAction(controllerName: "ShortUrls", actionName: "History");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
