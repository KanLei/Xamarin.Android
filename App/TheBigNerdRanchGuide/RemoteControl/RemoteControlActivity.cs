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

namespace RemoteControl
{
    [Activity(Label = "RemoteControlActivity", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class RemoteControlActivity : SingleFragmentActivity
    {

        protected override Fragment CreateFragment()
        {
            return new RemoteControlFragment();
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            base.OnCreate(bundle);
        }
    }
}