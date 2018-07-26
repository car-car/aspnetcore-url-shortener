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

        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// ���ͷs�����}�æs���Ʈw
        /// </summary>
        /// <param name="originalUrl"></param>
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

                return RedirectToAction(actionName: nameof(CreateSuccess), routeValues: new { id = shortUrl.Id });
            }

            return View(shortUrl);
        }

        /// <summary>
        /// ��ܵ��G
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateSuccess(int? id)
        {
            if (!id.HasValue) 
            {
                return NotFound();
            }

            var shortUrl = _service.GetById(id.Value);
            if (shortUrl == null) 
            {
                return NotFound();
            }

            ViewData["Path"] = ShortUrlHelper.Encode(shortUrl.Id);

            return View(shortUrl);
        }

        /// <summary>
        /// ����s���ᥴ�}���Ĥ@�ӭ���
        /// </summary>
        /// <param name="id"></param>
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
        /// �I�}�̭������s
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
