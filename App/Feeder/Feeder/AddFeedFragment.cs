
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
	public class AddFeedFragment : Fragment
	{
		public const string URL="Feeder.Url";

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Resource.Layout.fragment_add_feed, container, false);

			var urlEditText = v.FindViewById<EditText> (Resource.Id.urlEditText);

			var addUrlButton = v.FindViewById<Button> (Resource.Id.addUrlButton);
			addUrlButton.Click+= (sender, e) => {

				var intent = new Intent();
				intent.PutExtra(URL,urlEditText.Text);
				Activity.SetResult(Result.Ok,intent);

				Activity.Finish();
			};

			return v;
		}
	}
}

