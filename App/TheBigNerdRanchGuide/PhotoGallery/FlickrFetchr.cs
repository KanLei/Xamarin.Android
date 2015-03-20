using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;

namespace PhotoGallery
{
    public class FlickrFetchr
    {
        public const string TAG = "FlickrFetchr";

        private const string ENDPOINT = "http://api.flickr.com/services/rest";
        private const string API_KEY = "youApiKeyHere";
        private const string METHOD_GET_RECENT = "flickr.photos.getRecent";
        private const string PARAM_EXTRAS = "extras";

        private const string EXTRA_SMALL_URL = "url_s";

        public void FetchItems()
        {
            Uri uri = new Uri(ENDPOINT);
        }


        public async Task<byte[]> GetBytesAsync(string requestUrl)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("method", METHOD_GET_RECENT);
                client.DefaultRequestHeaders.Add("api_key", API_KEY);
                client.DefaultRequestHeaders.Add(PARAM_EXTRAS, EXTRA_SMALL_URL);
                return await client.GetByteArrayAsync(requestUrl).ConfigureAwait(false);
            }
        }

        public async Task<string> GetStringAsync(string requestUrl)
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(requestUrl).ConfigureAwait(false);
            }
        }
    }
}