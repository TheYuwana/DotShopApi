using System;

namespace DotShopApi
{
    public class Utils
    {
        private static Random rnd = new Random();

        // Generate a random ID.
        // TODO: better to use GUID
        public static long GenerateRandomId()
        {
            return rnd.Next(1, 99999);
        }

    }
}