using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Java.Util;

namespace CriminalIntent
{
    [Activity(Label = "CrimeActivity")]
    public class CrimeActivity : SingleFragmentActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            //return new CrimeFragment();
            UUID crimeId = (UUID)Intent.GetSerializableExtra(CrimeFragment.EXTRA_CRIME_ID);
            return CrimeFragment.NewInstance(crimeId);
        }
    }
}

