using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageScraper
{
    static class StringExtensionMethods
    {
        /// <summary>
        /// Extension
        /// </summary>
        /// <returns></returns>
        public static bool Contains(
            this List<string> target,
            string value,
            StringComparison comparison)
        {
            return target.FirstOrDefault(str => str.Equals(
                value,
                StringComparison.OrdinalIgnoreCase)) != null;
        }
    }
}
