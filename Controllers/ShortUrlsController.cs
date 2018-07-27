using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using UrlShortener.Helpers;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    public class ShortUrlsController : Controller
    {
        private readonly IShortUrlService _service;

        public ShortUrlsController(IShortUrlService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return RedirectToAction(actionName: nameof(Create));
        }

        /// <summary>
        /// Show create page
        /// </summary>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        public IActionResult Create(bool isPublic = true)
        {
            var model = new ShortUrl()
            {
                IsPrivate = !isPublic
            };

            return View(model);
        }

        /// <summary>
        /// Create page submit
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <param name="provider"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string originalUrl,string provider,string memo)
        {
            var shortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                Provider = provider,
                Memo = memo,
            };

            TryValidateModel(shortUrl);
            if (ModelState.IsValid)
            {
                _service.Save(shortUrl);

                return RedirectToAction(actionName: nameof(CreateSuccess), routeValues: new { path = shortUrl.Path });
            }

            return View(shortUrl);
        }

        /// <summary>
        /// Create Success page
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IActionResult CreateSuccess(string path)
        {
            var shortUrl = _service.GetByPath(path);
            if (shortUrl == null) 
            {
                return NotFound();
            }
            return View(shortUrl);
        }

        /// <summary>
        /// If user get the preview URL, goes to this page first
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpGet("/p/{path:required}", Name = "ShortUrls_Preview")]
        public IActionResult Perview(string path)
        { 
            var shortUrl = _service.GetByPath(path);
            if (shortUrl == null) 
            {
                return NotFound();
            }
            return View(shortUrl);
        }

        /// <summary>
        /// Old preview page
        /// TODO : will be removed
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IActionResult Perview(int? path)
        { 
            if (!path.HasValue) 
            {
                return NotFound();
            }

            var shortUrl = _service.GetById(path.Value);
            if (shortUrl == null) 
            {
                return NotFound();
            }
            return View(shortUrl);
        }

        /// <summary>
        /// Redirect to 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpGet("/ShortUrls/RedirectTo/{path:required}", Name = "ShortUrls_RedirectTo")]
        public IActionResult RedirectTo(int path)
        {
            if (path == 0) 
            {
                return NotFound();
            }
            var shortUrl = _service.GetById(path);
            if (shortUrl == null) 
            {
                return NotFound();
            }

            return Redirect(shortUrl.OriginalUrl);
        }
    }
}
