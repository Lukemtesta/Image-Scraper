using System.IO;

namespace ImageScraper.Helpers
{
    static class FileSystemHelpers
    {
        /// <summary>
        /// Create a directory.
        /// </summary>
        /// <param name="i_filepath"></param>
        /// <returns>True if directory did not exist and was created</returns>
        public static bool MakeDirectory(string i_dir_path)
        {
            bool ret = !Directory.Exists(i_dir_path);
            
            if (!ret)
            {
                Directory.CreateDirectory(i_dir_path);
            }

            return ret;
        }
    }
}
