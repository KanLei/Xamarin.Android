
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		private const int REQUESTCODE = 0;

		private ListView listView;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetHasOptionsMenu (true);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Resource.Layout.fragment_feed_list, container, false);

			listView = v.FindViewById<ListView> (Resource.Id.feedListView);
			listView.ItemClick += (sender, e) => {
				var intent = new Intent(Activity, typeof(FeedListItemActivity));
				intent.PutExtra(FeedListItemFragment.RSSFEEDINDEX, e.Position);
				StartActivity(intent);
			};

			UpdateListView ();

			return v;
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			base.OnCreateOptionsMenu (menu, inflater);
			inflater.Inflate (Resource.Menu.menu_feed_list, menu);
		}


		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.addFeedMenu:
				var intent = new Intent (Activity, typeof(AddFeedActivity));
				StartActivityForResult (intent, REQUESTCODE);
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}
		}

		public override async void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (resultCode == Result.Ok) {
				switch (requestCode) {
				case REQUESTCODE:
					string url = data.GetStringExtra (AddFeedFragment.URL);
					if (String.IsNullOrWhiteSpace (url))
						return;
					RssFeed rssFeed = await ParseXMLContent.GetRssFeedsAsync (url);
					if (rssFeed != null) {
						RssFeedLab.Get ().RssFeeds.Add (rssFeed);
						UpdateListView ();
					}
					
					break;
				default:
					base.OnActivityResult (requestCode, resultCode, data);
					break;
				}
			}
		}


		private void UpdateListView()
		{
			listView.Adapter = new FeedListAdapter (Activity, RssFeedLab.Get().RssFeeds);
		}
	}
}

