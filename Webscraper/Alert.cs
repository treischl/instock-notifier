using System;

namespace Webscraper
{
    public static class Alert
    {
        public static void WithBeeps()
        {
            Beep(3);
            Console.ReadKey();
        }

        private static void Beep(int times)
        {
            for (int i = 0; i < times; i++)
                Console.Beep(1000, 500);
        }
    }
}
