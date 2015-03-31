
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Feeder
{
	public class FeedListItemFragment : Fragment
	{
		public const string RSSFEEDINDEX = "RssFeed_Index";

		private int index;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			RetainInstance = true;
		}


		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v= inflater.Inflate (Resource.Layout.fragment_feed_list_item, container, false);

			ListView listView = v.FindViewById<ListView> (Resource.Id.feedListItemListView);
			listView.ItemClick += (sender, e) => {

			 	RssItem rssItem = ((FeedListItemAdapter)(listView.Adapter)).GetItem(e.Position);
				var uri= Android.Net.Uri.Parse(rssItem.Link);
				var intent = new Intent(Intent.ActionView, uri);
				StartActivity(intent);
			};


			index = Arguments.GetInt ("key_index");

			RssFeed rssFeed = RssFeedLab.Get ().RssFeeds [index];
			Activity.Title = rssFeed.Name;

			listView.Adapter = new FeedListItemAdapter (Activity, rssFeed.Items);

			return v;

		}


		public static FeedListItemFragment NewInstance(int index)
		{
			var feedListItemFragment = new FeedListItemFragment ();
			var bundle = new Bundle ();
			bundle.PutInt ("key_index", index);
			feedListItemFragment.Arguments = bundle;
			return feedListItemFragment;
		}


		private class FeedListItemAdapter:ArrayAdapter<RssItem>
		{
			private Activity activity;

			public FeedListItemAdapter (Activity activity, List<RssItem> rssItems)
				:base(activity, 0, rssItems)
			{
				this.activity = activity;
			}

			public override View GetView (int position, View convertView, ViewGroup parent)
			{
				if (convertView == null)
					convertView = activity.LayoutInflater.Inflate (Android.Resource.Layout.SimpleListItem2, parent, false);

				RssItem rssItem = GetItem (position);
				var text1= convertView.FindViewById<TextView> (Android.Resource.Id.Text1);
				text1.Text = rssItem.Title;
				var text2 = convertView.FindViewById<TextView> (Android.Resource.Id.Text2);
				text2.Text = rssItem.PubDate;

				return convertView;
			}
		}
	}
}

