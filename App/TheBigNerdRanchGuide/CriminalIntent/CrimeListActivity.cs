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

namespace CriminalIntent
{
    [Activity(Label = "CrimeListActivity", MainLauncher = true, Icon = "@drawable/icon")]
    public class CrimeListActivity : SingleFragmentActivity
    {
        // 从基类继承 OnCreate() 方法，并调用

        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return new CrimeListFragment();
        }
    }
}