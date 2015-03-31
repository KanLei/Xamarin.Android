
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
	[Activity (Label = "FeedListActivity", MainLauncher=true)]			
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

