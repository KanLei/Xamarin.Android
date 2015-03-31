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
using Android.Util;
using Android.Preferences;

namespace PhotoGallery
{
	[Activity(Label = "PhotoGalleryActivity", MainLauncher = true, LaunchMode=LaunchMode.SingleTop)]
	[IntentFilter(new string[]{"android.intent.action.SEARCH"})]
	[MetaData("android.app.searchable",Resource="@xml/searchable")]
    public class PhotoGalleryActivity : SingleFragmentActivity
    {
        protected override Fragment CreateFragment()
        {
            return new PhotoGalleryFragment();
        }

		protected override void OnNewIntent (Intent intent)
		{
			base.OnNewIntent (intent);


			var fragment = FragmentManager.FindFragmentById (Resource.Id.fragmentContainer) as PhotoGalleryFragment;

			if (Intent.ActionSearch.Equals(intent.Action)) {
				string query = intent.GetStringExtra (SearchManager.Query);
				Log.Info ("PhotoGalleryActivity", "Received a new search query: " + query);

				PreferenceManager.GetDefaultSharedPreferences (this)
					.Edit()
					.PutString("pref_search_query", query)
					.Commit();

			}
		}
    }
}