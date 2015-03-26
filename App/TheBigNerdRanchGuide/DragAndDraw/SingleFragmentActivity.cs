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

namespace DragAndDraw
{
    [Activity(Label = "SingleFragmentActivity")]
    public abstract class SingleFragmentActivity : Activity
    {
        protected abstract Fragment CreateFragment();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_fragment);

            if (FragmentManager.FindFragmentById(Resource.Id.fragmentContainer) == null)
                FragmentManager.BeginTransaction()
                    .Add(Resource.Id.fragmentContainer, CreateFragment())
                    .Commit();
        }
    }
}