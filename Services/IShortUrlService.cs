using System.Collections.Generic;
using System.Linq;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public interface IShortUrlService
    {
        IQueryable<ShortUrl> GetShortUrls(string metaTittle = null, string provider = null, string memo = null);

        ShortUrl GetById(int id);

        ShortUrl GetByPath(string path);

        ShortUrl GetByOriginalUrl(string originalUrl);

        int Save(ShortUrl shortUrl);
    }
}
