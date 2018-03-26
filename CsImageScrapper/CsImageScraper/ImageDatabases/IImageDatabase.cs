
using System;
using System.Collections.Generic;
using System.Xml;

namespace ImageScraper
{
    /// <summary>
    /// Data collection for a search item
    /// </summary>
    struct SearchTerm
    {
        public List<string> Tags { get; set; }
        public string SearchName { get; set; }
    }

    abstract class IImageDatabase
    {
        /// <summary>
        /// Accepted names of groups holding several tags in xml files holding search terms
        /// </summary>
        private static List<string> AcceptedTagGroupNames => 
            new List<String> { "FoodGroup", "Tags", "TagGroup" };

        /// <summary>
        /// Load database from publicly available URL
        /// </summary>
        /// <param name="i_xml_path"></param>
        /// <returns>Dictionary of [Tag] = Image url </returns>
        abstract protected Dictionary<string, List<string>> LoadDatabaseFromURL(string i_url_path);

        /// <summary>
        /// Save the photos returned from a result to disk
        /// </summary>
        /// <param name="i_results">Results of a Flickr API Photo Search</param>
        /// /// <param name="i_rootpath">Output root path on disk</param>
        /// <param name="i_min_size">Minimum size of the expected file</param>
        /// <returns>Number of successfully retrieved photos</returns>
        abstract public int SearchAndDownloadImages(
            SearchTerm i_search_term,
            string i_rootpath,
            int i_min_filesize = DatabaseDefines.PhotoNotAvailableSize);

        /// <summary>
        /// Load search tags and ids from the database CSV file with whitespace delimeter
        /// </summary>
        /// <param name="i_xml_path"></param>
        /// <returns></returns>
        public static Dictionary<string, List<SearchTerm>> LoadDatabaseSearchTagsFromXML(
            string i_xml_filepath = DatabaseDefines.DefaultTagXMLFilePath)
        {
            var ret = new Dictionary<string, List<SearchTerm>>();

            XmlTextReader reader = new XmlTextReader(i_xml_filepath);

            // optimise parsing
            string current = String.Empty;
            SearchTerm item = new SearchTerm();

            while (reader.Read())
            {
                if (AcceptedTagGroupNames.Contains(reader.Name))
                {
                    current = Convert.ToString(reader.GetAttribute("Value"));

                    if (!ret.ContainsKey(current))
                    {
                        throw new XmlException(
                            "XML file " + i_xml_filepath + " is in an invalid format");
                    }
                }
                else if (reader.Name == "Item")
                {
                    if(current == String.Empty)
                    {
                        throw new XmlException(
                            "XML file " + i_xml_filepath + " is in an invalid format");
                    }

                    item.Tags = new List<string> { reader.GetAttribute("Tag") };
                    item.SearchName = reader.GetAttribute("Name");
                    ret[current].Add(item);
                }
            }

            return ret;
        }
    }
}
