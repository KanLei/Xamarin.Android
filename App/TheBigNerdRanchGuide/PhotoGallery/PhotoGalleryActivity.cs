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

namespace PhotoGallery
{
    [Activity(Label = "PhotoGalleryActivity", MainLauncher = true)]
    public class PhotoGalleryActivity : SingleFragmentActivity
    {
        protected override Fragment CreateFragment()
        {
            return new PhotoGalleryFragment();
        }
    }
}