using System.Collections.Generic;

namespace Webscraper.DTO
{
    public class Product : Dictionary<string, string>
    {
        public KeyCollection Stores => Keys;
    }
}
