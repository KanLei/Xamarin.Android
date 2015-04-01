
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
using Android.Content.Res;

namespace Feeder
{
	[Activity (Label = "FeedList", MainLauncher=true)]			
	public class FeedListActivity : SingleFragmentActivity
	{
		#region implemented abstract members of SingleFragmentActivity

		protected override Fragment CreateFragment ()
		{
			return new FeedListFragment ();
		}

		#endregion
	}
}

