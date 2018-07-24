using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Models
{
    public class ShortUrlResult
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 原本車車網址
        /// </summary>
        public string OriginalUrl { get; set; }

        /// <summary>
        /// 翻譯過的車車網址
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// 誰提供的
        /// </summary>
        public string Provider { get; set; }
    }
}
