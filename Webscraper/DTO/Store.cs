using System.Linq;

namespace Webscraper.DTO
{
    public class Store
    {
        public string Name { get; set; }

        public string InStock { get; set; }

        public string NoStock { get; set; }

        public StockStatus GetStockStatus(Browser browser)
        {
            //if (browser.QuerySelectorAll(InStock).Any())
            //{
            //    return StockStatus.InStock;
            //}
            //if (browser.QuerySelectorAll(NoStock).Any())
            //{
            //    return StockStatus.OutOfStock;
            //}
            //return StockStatus.Unknown;
            return browser.QuerySelectorAll(InStock).Any()
                ? StockStatus.InStock
                : StockStatus.OutOfStock;
        }
    }
}
