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

namespace NerdLauncher
{
    [Activity(Label = "NerdLauncherActivity", MainLauncher = true)]
    [IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { Intent.CategoryHome,Intent.CategoryDefault })]
    public class NerdLauncherActivity : SingleFragmentActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return new NerdLauncherFragment();
        }
    }
}