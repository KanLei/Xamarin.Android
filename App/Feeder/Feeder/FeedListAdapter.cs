using System;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.App;

namespace Feeder
{
	public class FeedListAdapter:ArrayAdapter<RssFeed>
	{
		Activity activity;

		public FeedListAdapter (Activity activity, List<RssFeed> rssFeed):base(activity,0,rssFeed)
		{
			this.activity = activity;
		}


		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			if (convertView == null)
				convertView = activity.LayoutInflater.Inflate (Android.Resource.Layout.SimpleListItem2, parent, false);

			RssFeed rssFeed = GetItem (position);
			var text1= convertView.FindViewById<TextView> (Android.Resource.Id.Text1);
			text1.Text = rssFeed.Name;
			var text2 = convertView.FindViewById<TextView> (Android.Resource.Id.Text2);
			text2.Text = rssFeed.AddDateTime.ToString();

			return convertView;
		}
	}
}

