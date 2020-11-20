using System;
using System.Collections.Generic;
using System.Linq;

namespace Webscraper.DTO
{
    public class Product
    {
        private static readonly bool _ignoreErrors =
            Environment.GetCommandLineArgs().Contains("--ignore-errors");

        public string Name { get; set; }

        public IEnumerable<ProductListing> Listings { get; set; }

        private readonly Logger _logger = new Logger();

        public void CheckListings(Browser browser, IDictionary<string, Store> stores)
        {
            _logger.LogProduct(this);
            foreach (var listing in Listings)
            {
                browser.NavigateTo(listing.Url);
                var store = stores[listing.Store];
                var stockStatus = store.GetStockStatus(browser);
                _logger.LogStockStatus(store, stockStatus);
                HandleStockNotifications(stockStatus, listing);
                //Thread.Sleep(500);
            }
            //Thread.Sleep(1000);
        }

        private void HandleStockNotifications(StockStatus status, ProductListing listing)
        {
            switch (status)
            {
                case StockStatus.InStock when MinutesSinceLastSeenInStock() > 10:
                    listing.LastSeenInStock = DateTime.Now;
                    _logger.LogListingUrl(listing);
                    Alert.WithBeeps();
                    break;
                case StockStatus.Unknown when _ignoreErrors == false:
                    Alert.WithBeeps();
                    break;

            }

            double MinutesSinceLastSeenInStock() =>
                DateTime.Now.Subtract(listing.LastSeenInStock).TotalMinutes;
        }
    }
}
