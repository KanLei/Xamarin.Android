
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

namespace Feeder
{
	[Activity (Label = "FeedListItemActivity")]			
	public class FeedListItemActivity : SingleFragmentActivity
	{
		private int index;
		protected override void OnCreate (Bundle bundle)
		{
			index = Intent.GetIntExtra (FeedListItemFragment.RSSFEEDINDEX,0);

			base.OnCreate (bundle);
		}

		#region implemented abstract members of SingleFragmentActivity

		protected override Fragment CreateFragment ()
		{
			return FeedListItemFragment.NewInstance (index);
		}

		#endregion
	}
}

