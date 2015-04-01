
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
using Android.Content.PM;

namespace Feeder
{
	[Activity (Label = "PostPageActivity", ConfigurationChanges= ConfigChanges.KeyboardHidden|ConfigChanges.Orientation|ConfigChanges.ScreenSize)]			
	public class PostPageActivity : SingleFragmentActivity
	{
		private string urlLink;

		protected override void OnCreate (Bundle bundle)
		{
			urlLink = Intent.GetStringExtra (PostPageFragment.WEBVIEW_URL);

			base.OnCreate (bundle);
		}

		#region implemented abstract members of SingleFragmentActivity

		protected override Fragment CreateFragment ()
		{
			return PostPageFragment.NewInstance (urlLink);
		}

		#endregion


	}
}

