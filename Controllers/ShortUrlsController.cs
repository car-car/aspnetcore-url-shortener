using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [MiddlewareFilter(typeof(CultureMiddleware))]
    public class ShortUrlsController : Controller
    {
        private readonly IShortUrlService _shortUrlService;
        private readonly IUrlPerviewService _urlPerviewService;

        public ShortUrlsController(IShortUrlService service,
            IUrlPerviewService urlPerviewService)
        {
            _shortUrlService = service;
            _urlPerviewService = urlPerviewService;
        }

        #region ShortUrl

        /// <summary>
        /// Redirect to Create
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return RedirectToAction(actionName: nameof(Create));
        }

        /// <summary>
        /// Show History
        /// </summary>
        /// <returns></returns>
        public IActionResult History()
        {
            return View();
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
        /// <param name="isPrivate"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string originalUrl, string provider, string memo, bool isPrivate)
        {
            var urlPerview = new UrlPerview();
            try
            {
                urlPerview = _urlPerviewService.GetUrlPerview(originalUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var shortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                Provider = provider,
                Memo = memo,
                IsPrivate = isPrivate,
                MetaTitle = urlPerview?.Title,
                MetaDescription = urlPerview?.Description,
                PreviewImageUrl = urlPerview?.ListImages?.FirstOrDefault()
            };

            TryValidateModel(shortUrl);
            if (ModelState.IsValid)
            {
                _shortUrlService.Save(shortUrl);

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
            var shortUrl = _shortUrlService.GetByPath(path);
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
            var shortUrl = _shortUrlService.GetByPath(path);
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

            var shortUrl = _shortUrlService.GetById(path.Value);
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
            var shortUrl = _shortUrlService.GetById(path);
            if (shortUrl == null)
            {
                return NotFound();
            }

            return Redirect(shortUrl.OriginalUrl);
        }

        #endregion

        #region Json

        [HttpGet]
        public IActionResult GenerateUrlPerview(string url)
        {
            var result = _urlPerviewService.GetUrlPerview(url);
            return Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public JsonResult GetHistoryRecords(int? page, int? limit, string searchString = null)
        {
            if (page.HasValue && limit.HasValue)
            {
                //query all match
                var records = _shortUrlService.GetShortUrls(url: searchString);
                int total = records.Count();

                //paging
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);

                //return list
                return Json(new { records, total }, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver { NamingStrategy = null }
                });
            }

            return Json(null);
        }

        #endregion
    }
}
