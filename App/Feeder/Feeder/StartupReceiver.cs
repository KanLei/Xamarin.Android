
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;

namespace Feeder
{
	[BroadcastReceiver]
	[IntentFilter(new string[]{"android.intent.action.BOOT_COMPLETED"})]
	public class StartupReceiver : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
//			ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences (context);
//			int index = sharedPreferences.GetInt (PollService.RSSFEEDINDEX, 0);
//			string url = sharedPreferences.GetString (PollService.BlogUrl, null);
//			PollService.SetServiceAlarm (context, true, index, url);
		}
	}
}

