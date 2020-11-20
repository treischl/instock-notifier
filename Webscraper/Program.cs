using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Webscraper.DTO;

namespace Webscraper
{
    class Program
    {
        private static readonly IDictionary<string, Store> _stores = GetStores();
        private static readonly IEnumerable<Product> _products = GetProducts();

        static void Main(string[] _)
        {
            using var browser = new Browser();
            while (true)
            {
                CheckStock(browser);
                Thread.Sleep(2000);
            }
        }

        private static void CheckStock(Browser browser)
        {
            foreach (var product in _products)
            {
                product.CheckListings(browser, _stores);
            }
        }

        private static IDictionary<string, Store> GetStores() =>
            ParseJsonFile<Store>("stores.json", "stores").ToDictionary(x => x.Name);
        private static IEnumerable<Product> GetProducts() =>
            ParseJsonFile<Product>("products.json", "products");
        private static IEnumerable<T> ParseJsonFile<T>(string fileName, string key) =>
            JObject.Parse(File.ReadAllText(fileName))[key]
            .ToObject<IEnumerable<T>>();
    }
}
