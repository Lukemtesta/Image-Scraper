using ImageScraper;
using System;
using System.Threading.Tasks;

namespace CsImageScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load database and get Flickr API
            var flickrImageDatabase = new FlickrImageDatabase();
            var imageNetImageDatabase = new ImageNetImageDatabase();

            // TODO: Accept optional filepath from command line and pass instead of default param
            var searchDatabase = IImageDatabase.LoadDatabaseSearchTagsFromXML();

            foreach (var group in searchDatabase)
            {
                foreach (var item in group.Value)
                {
                    // TODO: Create singleton logger
                    Console.WriteLine(
                        "Downloading "
                        + item.SearchName
                        + " for tags "
                        + string.Join(",", item.Tags));

                    // TODO: Accept optional database root dir from command line and pass to func
                    // instead of default parameter
                    Task.Run(() => imageNetImageDatabase.SearchAndDownloadImages(item));
                    Task.Run(() => flickrImageDatabase.SearchAndDownloadImages(item));
                }
            }
        }
    }
}
