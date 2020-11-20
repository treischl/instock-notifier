using System;
using Webscraper.DTO;

namespace Webscraper
{
    public class Logger
    {

        public void LogProduct(Product product)
        {
            Console.WriteLine($"{product.Name}...");
        }

        public void LogStockStatus(Store store, StockStatus status)
        {
            Console.Write($"  - {store.Name}: ");
            switch (status)
            {
                case StockStatus.InStock:
                    LogInStock();
                    break;
                case StockStatus.OutOfStock:
                    LogOutOfStock();
                    break;
                default:
                    LogUnknownStock();
                    break;
            }

            static void LogInStock()
            {
                var currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(">> IN STOCK <<");
                Console.ForegroundColor = currentColor;
            }
            static void LogOutOfStock()
            {
                Console.WriteLine("no stock");
            }
            static void LogUnknownStock()
            {
                var currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.ForegroundColor = currentColor;
            }
        }

        public void LogListingUrl(ProductListing listing)
        {
            Console.WriteLine(listing.Url);
        }
    }
}
