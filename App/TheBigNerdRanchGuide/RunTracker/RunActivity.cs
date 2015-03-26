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

namespace RunTracker
{
    [Activity(Label = "RunActivity", MainLauncher = true)]
    public class RunActivity : SingleFragmentActivity
    {
        protected override Fragment CreateFragment()
        {
            return new RunFragment();
        }
    }
}