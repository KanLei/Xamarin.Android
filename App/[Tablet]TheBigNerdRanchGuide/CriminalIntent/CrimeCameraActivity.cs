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

namespace CriminalIntent
{
    [Activity(Label = "CrimeCameraActivity", ScreenOrientation = ScreenOrientation.Landscape)]
    public class CrimeCameraActivity : SingleFragmentActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Hide the window title
            RequestWindowFeature(WindowFeatures.NoTitle);
            // Hide the status bar and other OS-level chrome
            Window.AddFlags(WindowManagerFlags.Fullscreen);

            base.OnCreate(savedInstanceState);  // 显示调用基类的 OnCreate(...)
        }

        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return new CrimeCameraFragment();
        }
    }
}