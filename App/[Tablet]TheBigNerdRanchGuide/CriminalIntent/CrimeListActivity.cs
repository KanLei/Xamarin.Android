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
    [Activity(Label = "Crimes", MainLauncher = true, Icon = "@drawable/icon")]
    public class CrimeListActivity : SingleFragmentActivity, CrimeListFragment.ICallbacks, CrimeFragment.ICallbacks
    {
        // 从基类继承 OnCreate() 方法，并调用

        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return new CrimeListFragment();
        }

        protected override int GetLayoutResourceId()
        {
            return Resource.Layout.activity_masterdetail;
        }

        // CrimeListFragment.ICallbacks
        public void OnCrimeSelected(Crime crime)
        {
            if (FindViewById(Resource.Id.detailFragmentContainer) == null)
            {
                // Start an instance of CrimePagerActivity
                var i = new Intent(this, typeof(CrimePagerActivity));
                i.PutExtra(CrimeFragment.EXTRA_CRIME_ID, crime.Id);
                StartActivity(i);
            }
            else
            {
                Android.Support.V4.App.FragmentTransaction transaction = SupportFragmentManager.BeginTransaction();

                Android.Support.V4.App.Fragment oldDetail = SupportFragmentManager.FindFragmentById(Resource.Id.detailFragmentContainer);
                Android.Support.V4.App.Fragment newDetail = CrimeFragment.NewInstance(crime.Id);

                if (oldDetail != null)
                    transaction.Remove(oldDetail);
                transaction.Add(Resource.Id.detailFragmentContainer, newDetail);
                transaction.Commit();
            }
        }

        // CrimeFragment.ICallbacks
        public void OnCrimeUpdated(Crime crime)
        {
            var listFragment = SupportFragmentManager.FindFragmentById(Resource.Id.fragmentContainer) as CrimeListFragment;
            listFragment.UpdateUI();
        }
    }
}