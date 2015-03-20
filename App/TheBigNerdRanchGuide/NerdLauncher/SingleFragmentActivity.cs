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
using Android.Support.V4.App;

namespace NerdLauncher
{
    [Activity(Label = "SingleFragmentActivity")]
    public abstract class SingleFragmentActivity : FragmentActivity
    {
        protected abstract Android.Support.V4.App.Fragment CreateFragment();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.activity_fragment);

            if (SupportFragmentManager.FindFragmentById(Resource.Id.fragmentContainer) == null)
                SupportFragmentManager.BeginTransaction()
                    .Add(Resource.Id.fragmentContainer, CreateFragment())
                    .Commit();
        }
    }
}