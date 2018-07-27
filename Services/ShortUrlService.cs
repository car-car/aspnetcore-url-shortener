using System;
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
