
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

namespace PhotoGallery
{
	[BroadcastReceiver]
	[IntentFilter(new string[]{"android.intent.action.BOOT_COMPLETED"})]
	public class StartupReceiver : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			Toast.MakeText (context, "Received intent!", ToastLength.Short).Show ();
		}
	}
}

