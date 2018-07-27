using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }
        
        [Required]
        public string OriginalUrl { get; set; }

        public string MetaTitle { get; set; }

        public string MetaDescription { get; set; }

        public string PreviewImageUrl { get; set; }

        public string Provider { get; set; }

        public string Memo { get; set; }

        public string Path { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
