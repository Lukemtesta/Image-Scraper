using System.Collections.Generic;

namespace ImageScraper
{
    internal static class DatabaseDefines
    {
        /// <summary>
        /// Size of photo not available
        /// </summary>
        public const int PhotoNotAvailableSize = 4096;

        /// <summary>
        /// Root path of database on disk
        /// 
        /// TODO: Accept as command line argument
        /// </summary>
        public const string DefaultDatabaseRoot = "C:/Users/luke/Documents/Dataset/";

        /// <summary>
        /// Root path of flickr database on disk
        /// </summary>
        public const string DefaultDatabaseFlickrPath = DefaultDatabaseRoot + "Flickr";

        /// <summary>
        /// Root path of imagenet database on disk
        /// </summary>
        public const string DefaultDatabaseImageNetPath = DefaultDatabaseRoot + "ImageNet";

        /// <summary>
        /// TODO: Make this a required command line argument in Program.cs
        /// </summary>
        public const string DefaultTagXMLFilePath =
            "C:/Users/luke/Downloads/database/SearchDatabase.xml";

        /// <summary>
        /// TODO: Make this a required command line argument in Program.cs
        /// </summary>
        public static string ImageNetSynetDatabaseFilePath =>
            "C:/Users/luke/Downloads/database/ImageNetSynetDatabase.txt";
    }
}
