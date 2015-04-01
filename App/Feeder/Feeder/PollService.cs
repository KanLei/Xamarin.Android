using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Net;
using Android.Preferences;
using System.Collections.Generic;

namespace Feeder
{
	[Service]
	public class PollService : IntentService
	{
		private const string TAG = "PollService";
		public const string RSSFEEDINDEX = "PollService.RssFeedIndex";
		public const string BlogUrl = "PollService.BlogUrl";

		private const int POLL_INTERVAL = 1000 * 15;

		private static int rssFeedIndex;
		private static string blogUrl;


		public PollService () : base (TAG)
		{
		}


		#region implemented abstract members of IntentService

		protected override async void OnHandleIntent (Intent intent)
		{
			RssFeed newRssFeed = await ParseXMLContent.GetRssFeedsAsync (blogUrl);
			if (newRssFeed.Items != null && newRssFeed.Items.Count > 0) {
				List<RssFeed> rssFeeds = RssFeedLab.Get ().RssFeeds;
				RssFeed oldRssFeed = rssFeeds [rssFeedIndex];

//				if (!newRssFeed.Items [0].Title.Equals (oldRssFeed.Items [0].Title)) {
					rssFeeds [rssFeedIndex] = newRssFeed;

					var i = new Intent (this, typeof(FeedListItemActivity));
					i.PutExtra (FeedListItemFragment.RSSFEEDINDEX, rssFeedIndex);
					//i.AddFlags (ActivityFlags.NewTask);
					if (PendingIntent.GetActivity (this, 0, i, PendingIntentFlags.NoCreate) != null)
						return;

					PendingIntent pi = PendingIntent.GetActivity (this, 0, i, PendingIntentFlags.CancelCurrent);
					var notificationManager = GetSystemService (Context.NotificationService) as NotificationManager;

					Notification notification = new Notification.Builder (this)
						.SetDefaults (NotificationDefaults.All)  // 注意这个要设置
						.SetTicker ("Blog Has New Post")
						.SetSmallIcon (Resource.Drawable.Icon)
						.SetContentTitle (newRssFeed.Name)
						.SetContentText ("New Post")
						.SetAutoCancel (true)
						.SetContentIntent (pi)
						.Build ();

					notificationManager.Notify (0, notification);
//				}
			}
		}

		#endregion


		public static void SetServiceAlarm (Context context, bool isOn, int index = 0, string url = null)
		{
			var i = new Intent (context, typeof(PollService));
			PendingIntent pi = PendingIntent.GetService (context, 0, i, 0);

			var alarmManager = context.GetSystemService (Context.AlarmService) as AlarmManager;

			if (isOn) {
				rssFeedIndex = index;
				blogUrl = url;
				alarmManager.SetRepeating (AlarmType.Rtc, 0, POLL_INTERVAL, pi);
			} else {
				alarmManager.Cancel (pi);
				pi.Cancel ();
			}

			ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences (context);
			sharedPreferences.Edit ().PutInt (RSSFEEDINDEX, rssFeedIndex).PutString (BlogUrl, blogUrl).Commit ();
		}

		public static bool IsServiceAlarmOn (Context context)
		{
			var i = new Intent (context, typeof(PollService));
			// NoCreate: this flag says that if the PendingIntent dose not already exist,
			// return null instead of creating it.
			PendingIntent pi = PendingIntent.GetService (context, 0, i, PendingIntentFlags.NoCreate);
			return pi != null;
		}
	}
}

