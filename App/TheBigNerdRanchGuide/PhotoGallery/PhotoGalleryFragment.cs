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
using System.Threading.Tasks;
using Android.Preferences;

namespace PhotoGallery
{
    public class PhotoGalleryFragment : Fragment
    {
        private GridView gridView;
		private JavaList<GalleryItem> items;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RetainInstance = true;
			SetHasOptionsMenu (true);

			PollService.SetServiceAlarm (Activity, true);


			items = new JavaList<GalleryItem> {
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
				new GalleryItem ("hsdlf"),
			};


        }

		public override void OnPrepareOptionsMenu (IMenu menu)
		{
			base.OnPrepareOptionsMenu (menu);

			IMenuItem toggleItem = menu.FindItem(Resource.Id.menu_item_toggle_polling);
			if (PollService.IsServiceAlarmOn(Activity)) {
				toggleItem.SetTitle(Resource.String.stop_polling);
			} else {
				toggleItem.SetTitle(Resource.String.start_polling);
			}
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			base.OnCreateOptionsMenu (menu, inflater);
			inflater.Inflate (Resource.Menu.fragment_photo_gallery, menu);
		}


		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.menu_item_search:
				Activity.OnSearchRequested ();
				return true;
			case Resource.Id.menu_item_clear:
				PreferenceManager.GetDefaultSharedPreferences (Activity).Edit ().PutString ("pref_search_query", null).Commit ();
				return true;
			case Resource.Id.menu_item_toggle_polling:
				if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb) {
					Activity.InvalidateOptionsMenu ();
				}
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}
		}

		private void SetupAdapter ()
		{
			if (Activity == null || gridView == null)
				return;

			if (items != null) {
				gridView.Adapter = new GalleryItemAdapter (Activity, items);
			}
			else {
				gridView.Adapter = null;
			}
		}

		public async Task Test()
		{
			string content = await new FlickrFetchr ().GetStringAsync ("http://api.flickr.com/services/rest/?method=flickr.photos.getRecent&api_key=68c3a95c35bcca1f8f5e9964e1fe0a42");
			Log.Info ("PhotoGalleryFragment", content);
		}


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.fragment_photo_gallery, container, false);
            gridView = v.FindViewById<GridView>(Resource.Id.gridView);

			SetupAdapter ();

            return v;
        }

		private class GalleryItemAdapter:ArrayAdapter<GalleryItem>
		{
			private Activity activity;

			public GalleryItemAdapter (Activity activity, JavaList<GalleryItem> items)
				:base(activity,0,items)
			{
				this.activity = activity;
			}

			public override View GetView (int position, View convertView, ViewGroup parent)
			{
				convertView = convertView ?? activity.LayoutInflater.Inflate (Resource.Layout.gallery_item, null);

				ImageView imageView = convertView.FindViewById<ImageView> (Resource.Id.gallery_item_imageView);
				imageView.SetImageResource (Resource.Drawable.Icon);

				return imageView;
			}
		}

    }


}