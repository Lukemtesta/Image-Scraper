using System;
using System.Net;

namespace ImageScraper
{
    internal class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 300000;
            return w;
        }

        /// <summary>
        /// Download file with error handling
        /// </summary>
        /// <param name="address"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public new bool DownloadFile(string address, string filename)
        {
            bool ret = true;

            try
            {
                DownloadFile(address, filename);
            }
            catch (WebException)
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Download string with error handling
        /// </summary>
        /// <param name="address"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool DownloadString(ref string output, string url)
        {
            bool ret = true;

            try
            {
                output = DownloadString(url);
            }
            catch (WebException)
            {
                ret = false;
            }

            return ret;
        }
    }
}
