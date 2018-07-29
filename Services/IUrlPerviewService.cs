using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public interface IUrlPerviewService
    {
        /// <summary>
        /// Get preview data from image
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        UrlPerview GetUrlPerview(string url);
    }
}
