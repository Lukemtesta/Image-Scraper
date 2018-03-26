using FlickrNet;
using ImageScraper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageScraper
{
    class FlickrImageDatabase : IImageDatabase
    {
        /// <summary>
        /// Target file extension of downloaded file
        /// </summary>
        private const string TargetFileExtension = ".jpg";

        /// <summary>
        /// Download search terms
        /// </summary>
        public PhotoSearchOptions SearchOptions { get; set; } = null;

        public FlickrImageDatabase()
        {}

        public FlickrImageDatabase(FlickrImageDatabase copy)
        {}

        /// <summary>
        /// Load database from publicly available URL
        /// </summary>
        /// <param name="i_xml_path"></param>
        /// <returns>Dictionary of [Tag] = Image url </returns>
        override protected Dictionary<string, List<string>> LoadDatabaseFromURL(string i_xml_path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create search option for Flickr database
        /// </summary>
        /// <param name="i_name">Photo name</param>
        /// <param name="i_tags">Photo Tags</param>
        /// <param name="i_max_result_count">Number of top results returned</param>
        /// <returns>Search results</returns>
        public PhotoSearchOptions CreatePhotoSearchOption(
            SearchTerm i_search_term,
            int i_max_result_count = 500)
        {
            PhotoSearchOptions options = new PhotoSearchOptions();

            options.Extras = PhotoSearchExtras.All;
            options.SortOrder = PhotoSearchSortOrder.Relevance;
            options.Tags = string.Join(",", i_search_term.Tags);
            options.Text = i_search_term.SearchName;
            options.TagMode = TagMode.AllTags;
            options.PerPage = i_max_result_count;

            return options;
        }

        public PhotoSearchOptions CreatePhotoSearchOption(
            string i_name = "",
            List<string> i_tags = null,
            int i_max_result_count = 500)
        {
            SearchTerm options = new SearchTerm();

            options.SearchName = i_name;
            options.Tags = i_tags;

            return CreatePhotoSearchOption(options, i_max_result_count);
        }

        /// <summary>
        /// Apply search on Flickr database and retrieve result metadata
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private PhotoCollection ApplyPhotoSearch(PhotoSearchOptions options)
        {
            Flickr api = FlickrManager.GetInstance();

            return api.PhotosSearch(options);
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
            string i_rootpath = DatabaseDefines.DefaultDatabaseFlickrPath,
            int i_min_filesize = DatabaseDefines.PhotoNotAvailableSize)
        {
            SearchOptions = CreatePhotoSearchOption(i_search_term);

            if (SearchOptions == null)
            {
                return 0;
            }

            var results = ApplyPhotoSearch(SearchOptions).Where(o => o.DoesLargeExist);

            // Create database directory
            string database_path = Path.Combine(i_rootpath, i_search_term.SearchName);
            FileSystemHelpers.MakeDirectory(database_path);

            // Download image
            int invalid_count = 0;
            var client = new MyWebClient();

            foreach (var result in results)
            {
                string filename = Path.Combine(
                    database_path,
                    i_search_term.SearchName + "_" + result.PhotoId + TargetFileExtension);

                if(!client.DownloadFile(result.LargeUrl, filename))
                { 
                    ++invalid_count;
                }
            }

            return results.Count() - invalid_count;
        }
    }
}
