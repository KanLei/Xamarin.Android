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

namespace CriminalIntent
{
    public abstract class SingleFragmentActivity : FragmentActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_fragment);
            var fragment = SupportFragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
            if (fragment == null)
            {
                fragment = CreateFragment();
                SupportFragmentManager.BeginTransaction()
                    .Add(Resource.Id.fragmentContainer, fragment)
                    .Commit();
            }
        }

        protected abstract Android.Support.V4.App.Fragment CreateFragment();
    }
}