using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace HelloMoon
{
    [Activity(Label ="@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class HelloMoonActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_hello_moon);

        }
    }
}

