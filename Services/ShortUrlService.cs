using System;
using System.Collections.Generic;
using System.Linq;
using UrlShortener.Data;
using UrlShortener.Helpers;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public class ShortUrlService : IShortUrlService
    {
        private readonly UrlShortenerContext _context;

        public ShortUrlService(UrlShortenerContext context)
        {
            _context = context;
        }

        public IQueryable<ShortUrl> GetShortUrls(string url = null, string metaTittle = null,string provider = null,string memo = null, bool showPublicOnly = true)
        {
            var searchList = _context.ShortUrls.AsQueryable();

            if (url != null)
            {
                searchList = searchList.Where(x => x.OriginalUrl.Contains(url));
            }

            if (metaTittle != null)
            {
                searchList = searchList.Where(x => x.MetaTitle.Contains(metaTittle));
            }

            if (provider != null)
            {
                searchList = searchList.Where(x => x.Provider.Contains(provider));
            }

            if (memo != null)
            {
                searchList = searchList.Where(x => x.Memo.Contains(memo));
            }

            if (showPublicOnly)
            {
                searchList = searchList.Where(x => x.IsPrivate == false);
            }

            searchList = searchList.OrderByDescending(x => x.Id);

            return searchList;
        }

        public ShortUrl GetById(int id)
        {
            return _context.ShortUrls.Find(id);
        }

        public ShortUrl GetByPath(string path)
        {
            return _context.ShortUrls.FirstOrDefault(x=>x.Path == path);
        }

        public ShortUrl GetByOriginalUrl(string originalUrl)
        {
            foreach (var shortUrl in _context.ShortUrls) {
                if (shortUrl.OriginalUrl == originalUrl) {
                    return shortUrl;
                }
            }

            return null;
        }

        public int Save(ShortUrl shortUrl)
        {
            //insert
            shortUrl.CreateDate = DateTime.UtcNow;
            _context.ShortUrls.Add(shortUrl);
            _context.SaveChanges();

            //update path
            shortUrl.Path = ShortUrlHelper.Encode(shortUrl.Id);
            _context.ShortUrls.Update(shortUrl);
            _context.SaveChanges();

            return shortUrl.Id;
        }
    }
}
