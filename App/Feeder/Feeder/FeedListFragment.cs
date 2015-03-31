
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Feeder
{
	public class FeedListFragment : Fragment
	{
		private const int REQUESTCODE=0;

		private ListView listView;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Resource.Layout.fragment_feed_list, container, false);

			listView = v.FindViewById<ListView> (Resource.Id.feedListView);
			listView.ItemClick += (sender, e) => {
				var intent = new Intent(Activity, typeof(FeedListItemActivity));
				intent.PutExtra(FeedListItemFragment.RSSFEEDINDEX, e.Position);
				StartActivity(intent);
			};


			var addFeedButton = v.FindViewById<Button> (Resource.Id.addFeedButton);
			addFeedButton.Click += (sender, e) => {
				var intent = new Intent(Activity,typeof(AddFeedActivity));
				StartActivityForResult(intent,REQUESTCODE);
			};

			UpdateListView ();

			return v;
		}


		public override async void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (resultCode == Result.Ok && requestCode == REQUESTCODE) {
				string url= data.GetStringExtra (AddFeedFragment.URL);
				if (String.IsNullOrWhiteSpace (url))
					return;

				if (!url.Contains ("http://")) {
					url = "http://" + url;
				}

				string content= await GetStringAsync(url);
				ParseXmlContent (content);


			}

			base.OnActivityResult (requestCode, resultCode, data);
		}

		private async Task<string> GetStringAsync(string url)
		{
			using(var client = new HttpClient())
			{
				return await client.GetStringAsync (url);
			}
		}

		private void ParseXmlContent(string content)
		{
			XDocument doc= XDocument.Parse (content);
			XElement element= doc.Descendants ("channel").FirstOrDefault ();
			if (element != null) {
				string title= element.Element ("title").Value;

				var rssFeed = new RssFeed (title);

				rssFeed.Items = (from item in doc.Descendants ("item")
				                 select new RssItem () {
						Title = item.Element ("title").Value,
						Link = item.Element ("link").Value,
						PubDate = item.Element ("pubDate").Value,
					Creator = GetCreator (item)
				}).ToList ();

				RssFeedLab.Get ().RssFeeds.Add (rssFeed);
				UpdateListView ();
			}
		}

		private string GetCreator(XElement item)
		{
			string author = string.Empty;
			XNamespace dc = "http://purl.org/dc/elements/1.1/";
			try
			{
				author = item.Element("author")==null?null:item.Element("author").Value;
				if (author == null)
				{
					author = item.Element(dc + "creator").Value;
				}
			}
			catch (Exception ex)
			{
				Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
			}

			return author;
		}

		private void UpdateListView()
		{
			listView.Adapter = new FeedListAdapter (Activity, RssFeedLab.Get().RssFeeds);
		}
	}
}

