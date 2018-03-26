using FlickrNet;

namespace ImageScraper
{
    public class FlickrManager
    {
        public const string ApiKey = "3a68f22971d8d66b521b362c312c175c";
        public const string SharedSecret = "b2acf0fb7910be24";

        public static Flickr GetInstance()
        {
            return new Flickr(ApiKey, SharedSecret);
        }

        public static Flickr GetAuthInstance()
        {
            return new Flickr(ApiKey, SharedSecret);

            // Required for 3.17
            //f.OAuthAccessToken = OAuthToken.Token;
            //f.OAuthAccessTokenSecret = OAuthToken.TokenSecret;
            //return f;
        }

        // Required for 3.17
        //public static OAuthAccessToken OAuthToken
        //{
        //    get
        //    {
        //        return Properties.Settings.Default.OAuthToken;
        //    }
        //    set
        //    {
        //        Properties.Settings.Default.OAuthToken = value;
        //        Properties.Settings.Default.Save();
        //    }
        //}
    }
}
