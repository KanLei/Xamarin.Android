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
using Android.Content.PM;

namespace NerdLauncher
{
    public class NerdLauncherFragment : Android.Support.V4.App.ListFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var startupIntent = new Intent(Intent.ActionMain);
            startupIntent.AddCategory(Intent.CategoryLauncher);

            var pm = Activity.PackageManager;
            List<ResolveInfo> activities = pm.QueryIntentActivities(startupIntent, 0).OrderBy(x => x.LoadLabel(pm)).ToList();

            this.ListAdapter = new ResolveInfoAdapter(Activity, activities);
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);

            var adapter = ListAdapter as ResolveInfoAdapter;
            ActivityInfo activityInfo = adapter.GetItem(position).ActivityInfo;

            if (activityInfo != null)
            {
                var i = new Intent(Intent.ActionMain);
                i.SetClassName(activityInfo.ApplicationInfo.PackageName, activityInfo.Name);
                i.AddFlags(ActivityFlags.NewTask);
                StartActivity(i);
            }
        }
    }

    public class ResolveInfoAdapter : ArrayAdapter<ResolveInfo>
    {
        private Activity activity;
        public ResolveInfoAdapter(Activity activity, IList<ResolveInfo> resolveInfos)
            : base(activity, 0, resolveInfos)
        {
            this.activity = activity;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
                convertView = activity.LayoutInflater.Inflate(Resource.Layout.app_list_item, parent, false);

            var iconImageView = convertView.FindViewById<ImageView>(Resource.Id.appIconImageView);
            iconImageView.SetImageDrawable(GetItem(position).LoadIcon(activity.PackageManager));
            var lableNameTextView = convertView.FindViewById<TextView>(Resource.Id.appNameTextView);
            lableNameTextView.Text = GetItem(position).LoadLabel(activity.PackageManager);

            return convertView;
        }
    }
}