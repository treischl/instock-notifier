using System;

namespace Webscraper.DTO
{
    public class ProductListing
    {
        public string Store { get; set; }

        public string Url { get; set; }

        public DateTime LastSeenInStock { get; set; }
    }
}