using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using ModernHttpClient;

using Android.App;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Util;
using System.Net;
using System.Net.Http;

namespace Feeder
{
	public class ParseXMLContent
	{
		public ParseXMLContent ()
		{
		}

		private static async Task<string> GetStringAsync(string url)
		{
			using(var client = new HttpClient(new OkHttpNetworkHandler()))
			{
				return await client.GetStringAsync(url).ConfigureAwait(false);
			}


			#region
//			string content = null;
//			var request = WebRequest.Create (url);
//			request.BeginGetResponse ((ar) => {
//				var httpWebRequest = ar.AsyncState as HttpWebRequest;
//
//				var response = httpWebRequest.EndGetResponse (ar);
//				var reader = new System.IO.StreamReader (response.GetResponseStream ());
//				content = reader.ReadToEnd();
//				}, request);
//
//			while (content == null) {  }
//
//			return content;
			#endregion
		}

		private static string GetCreator(XElement item)
		{
			string author = string.Empty;
			XNamespace dc = "http://purl.org/dc/elements/1.1/";
			try
			{
				author = item.Element("author")==null?null:item.Element("author").Value;
				author = author ?? item.Element(dc + "creator").Value;
			}
			catch (Exception ex)
			{
				throw;
			}

			return author;
		}

		public static async Task<RssFeed> GetRssFeedsAsync(string url)
		{
			if (!url.Contains ("http://")) {
				url = "http://" + url;
			}

			string content = await GetStringAsync(url).ConfigureAwait(false);
			XDocument doc= XDocument.Parse (content);
			XElement element= doc.Descendants ("channel").FirstOrDefault ();
			if (element != null) {
				string title= element.Element ("title").Value;

				var rssFeed = new RssFeed (title, url);
				rssFeed.Items = (from item in doc.Descendants ("item")
					select new RssItem () {
						Title = item.Element ("title").Value,
						Link = item.Element ("link").Value,
						PubDate = item.Element ("pubDate").Value,
						Creator = GetCreator (item)
					}).ToList ();

				return rssFeed;
			}
			return null;
		}
	}
}

