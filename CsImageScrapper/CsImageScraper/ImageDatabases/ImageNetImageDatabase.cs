using ImageScraper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ImageScraper
{
    class ImageNetImageDatabase : IImageDatabase
    {
        /// <summary>
        /// Link to ImageNet database class guid map
        /// </summary>
        private readonly static string ImageNetImageDatabaseBaseURL =
            "http://www.image-net.org/api/text/imagenet.synset.geturls?wnid=";

        /// <summary>
        /// Image Tags. First tag in synetID map is the ID category, extra tags 
        /// are alternative descriptions
        /// </summary>
        private Dictionary<string, List<string>> ImageNetSynetTags =
            new Dictionary<string, List<string>>();

        public ImageNetImageDatabase()
        {
            ImageNetSynetTags = LoadDatabaseFromURL(ImageNetImageDatabaseBaseURL);
        }

        public ImageNetImageDatabase(ImageNetImageDatabase copy)
        {
            ImageNetSynetTags = copy.ImageNetSynetTags;
        }

        /// <summary>
        /// Load database from publicly available URL
        /// </summary>
        /// <param name="i_xml_path"></param>
        /// <returns>Dictionary of [Tag] = Image url </returns>
        override protected Dictionary<string, List<string>> LoadDatabaseFromURL(string i_url_path)
        {
            Dictionary<string, List<string>> ret =
                    new Dictionary<string, List<string>>();

            using (var file_handler = new StreamReader(File.OpenRead(i_url_path)))
            {
                while (!file_handler.EndOfStream)
                {
                    string line = file_handler.ReadLine();

                    // remove whitespace and split CSV line
                    Regex.Replace(line, @"\s+", "");
                    var values = line.Split(
                            new string[] { ", " },
                            StringSplitOptions.RemoveEmptyEntries).ToList();

                    ret[values[0]] = values.GetRange(1, values.Count() - 1);
                }
            }

            return ret;
        }

        /// <summary>
        /// Save the photos returned from a result to disk
        /// </summary>
        /// <param name="i_collection">Results of a Flickr API Photo Search</param>
        /// /// <param name="i_rootpath">Output root path on disk</param>
        /// <param name="i_min_size">Minimum size of the expected file</param>
        /// <returns>Number of successfully retrieved photos</returns>
        override public int SearchAndDownloadImages(
            SearchTerm i_search_term,
            string i_rootpath = DatabaseDefines.DefaultDatabaseImageNetPath,
            int i_min_filesize = DatabaseDefines.PhotoNotAvailableSize)
        {
            var synetID = FindSynetIDFromSearchTerm(i_search_term.SearchName);
            var tagImageURLs = ExtractSynetURLs(synetID);

            if (tagImageURLs == null)
            {
                return 0;
            }

            // Create database directory
            string database_path = Path.Combine(i_rootpath, i_search_term.SearchName);
            FileSystemHelpers.MakeDirectory(database_path);

            // Download image
            int invalid_count = 0;
            var client = new MyWebClient();

            foreach (var url in tagImageURLs)
            {
                string filename = Path.Combine(
                    database_path,
                    Path.GetFileName(url));

                if (!client.DownloadFile(url, filename))
                {
                    ++invalid_count;
                }
            }

            return tagImageURLs.Count() - invalid_count;
        }

        /// <summary>
        /// Extract the synetID tag from the ID category. 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="is_category"></param>
        /// <returns></returns>
        private string FindSynetIDFromSearchTerm(
            string tag,
            bool is_category = true)
        {
            // First tag in synetID map is the ID category, extra tags are alternative descriptions
            if (is_category)
            {
                return ImageNetSynetTags.FirstOrDefault(o => o.Value[0].Equals(
                tag,
                StringComparison.OrdinalIgnoreCase)).Key;
            }
            else
            {
                return ImageNetSynetTags.FirstOrDefault(o => o.Value.Contains(
                tag,
                StringComparison.OrdinalIgnoreCase)).Key;
            }
        }

        /// <summary>
        /// Extract all image urls related to a synet key
        /// </summary>
        /// <param name="i_synet_key"></param>
        /// <returns>List of synet urls. Null if key has no images</returns>
        private List<string> ExtractSynetURLs(string i_synet_key)
        {
            List<string> ret = null;

            // DownloadString is not thread safe :'(... SLLOOOOWWWWW
            string urls = "";
            using (MyWebClient client = new MyWebClient())
            {
                client.DownloadString(ref urls, ImageNetImageDatabaseBaseURL + i_synet_key);
            }

            // Ignore empty keys
            if (urls.Contains("http"))
            {
                ret = urls.Replace("\r", "").Replace("\n", ",").Split(',').ToList();
            }

            return ret;
        }
    }
}
