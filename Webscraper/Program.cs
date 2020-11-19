using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Webscraper.DTO;

namespace Webscraper
{
    class Program
    {
        private static ChromeDriver _driver = null;
        private static Dictionary<string, Store> _stores = null;
        private static Dictionary<string, Product> _products = null;

        private static bool _ignoreErrors = true;

        static void Main(string[] args)
        {
            try
            {
                _ignoreErrors = args.Contains("--ignore-errors");
                var options = SetChromeOptions();
                _driver = new ChromeDriver(options);
                while (true)
                {
                    CheckStock();
                    Thread.Sleep(2000);
                }
            }
            finally
            {
                _driver.Close();
            }
        }

        private static ChromeOptions SetChromeOptions()
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless");
            //options.AddArgument("--no-sandbox");
            //options.AddArgument("--disable-dev-shm-usage");
            //options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            //options.AddUserProfilePreference("profile.managed_default_content_settings.javascript", 2);
            return options;
        }

        private static void CheckStock()
        {
            _stores = GetStores();
            _products = GetProducts();
            foreach (var (name, product) in _products)
            {
                CheckProduct(name, product);
            }
        }

        private static void CheckProduct(string name, Product product)
        {
            Console.WriteLine($"{name}...");
            foreach (var (store, url) in product)
            {
                CheckStore(store, url);
            }
            Thread.Sleep(500);
        }

        private static void CheckStore(string name, string url)
        {
            _driver.Url = url;
            var store = _stores[name];
            Console.Write($"  - {name}: ");
            var inStock = _driver.FindElements(By.CssSelector(store.InStock)).Any();
            var noStock = _driver.FindElements(By.CssSelector(store.NoStock)).Any();
            if (inStock)
            {
                WriteInStock(url);
            }
            else if (noStock)
            {
                Console.WriteLine("no stock");
            }
            else
            {
                WriteError();
            }
        }

        private static void WriteInStock(string url)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(">> IN STOCK <<");
            Console.ForegroundColor = currentColor;
            Console.WriteLine(url);
            Beep(3);
            Console.ReadKey();
        }

        private static void WriteError()
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR");
            Console.ForegroundColor = currentColor;

            if (_ignoreErrors == false)
            {
                Beep(3);
                Console.ReadKey();
            }
        }

        private static Dictionary<string, Product> GetProducts() =>
            GetJson<Dictionary<string, Product>>("products.json");
        private static Dictionary<string, Store> GetStores() =>
            GetJson<Dictionary<string, Store>>("stores.json");
        private static T GetJson<T>(string filename)
        {
            using var file = OpenFile(filename);
            var serializer = new JsonSerializer();
            var json = (T)serializer.Deserialize(file, typeof(T));
            return json;
        }
        private static StreamReader OpenFile(string filename)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var exeDirectory = Path.GetDirectoryName(path);
            return File.OpenText(Path.Combine(exeDirectory, filename));
        }

        private static void Beep(int times)
        {
            for (int i = 0; i < times; i++)
                Console.Beep(1000, 500);
        }
    }
}
