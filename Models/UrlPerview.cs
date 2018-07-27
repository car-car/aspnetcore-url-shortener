using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Models
{
    public class UrlPerview
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<string> ListImages { get; set; }
    }
}
