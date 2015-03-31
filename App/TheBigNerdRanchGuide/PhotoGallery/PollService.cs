using System;

using Android.App;
using Android.Content;
using Android.Net;
using Android.Util;


namespace PhotoGallery
{
	[Service]
	public class PollService:IntentService
	{
		private const string TAG = "PollService";
		private const int POLL_INTERVAL = 1000 * 15;  // 15 seconds

		public PollService ():base(TAG)
		{
		}

		#region implemented abstract members of IntentService

		protected override void OnHandleIntent (Intent intent)
		{
			var cm = GetSystemService (Context.ConnectivityService) as ConnectivityManager;
			bool isNetworkAvailable = cm.BackgroundDataSetting  // in older versions of Android
			                          && cm.ActiveNetworkInfo != null;  // in Andorid 4.0 Ice Cream Sandwich
			if (!isNetworkAvailable)
				return;

			Log.Info (TAG, "PollService is running...");

		}

		#endregion


		public static void SetServiceAlarm(Context context, bool isOn)
		{
			var i = new Intent (context, typeof(PollService));
			PendingIntent pi = PendingIntent.GetService (context, 0, i, 0);

			var alarmManager = context.GetSystemService (Context.AlarmService) as AlarmManager;

			if (isOn) {
				alarmManager.SetRepeating (AlarmType.Rtc, 0, POLL_INTERVAL, pi);
			} else {
				alarmManager.Cancel (pi);
				pi.Cancel ();
			}
		}

		public static bool IsServiceAlarmOn(Context context)
		{
			var i = new Intent (context, typeof(PollService));
			// NoCreate: this flag says that if the PendingIntent dose not already exist,
			// return null instead of creating it.
			PendingIntent pi = PendingIntent.GetService (context, 0, i, PendingIntentFlags.NoCreate);
			return pi != null;
		}
	}
}

