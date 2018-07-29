using System.Collections.Generic;
using System.Linq;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public interface IShortUrlService
    {
        IQueryable<ShortUrl> GetShortUrls(string metaTittle = null, string provider = null, string memo = null,bool showPublicOnly = true);

        ShortUrl GetById(int id);

        ShortUrl GetByPath(string path);

        ShortUrl GetByOriginalUrl(string originalUrl);

        int Save(ShortUrl shortUrl);
    }
}
