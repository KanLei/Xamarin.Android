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
using Android.Support.V4.View;
using Java.Util;
using Android.Support.V4.App;

namespace CriminalIntent
{
    [Activity(Label = "CrimePagerActivity")]
    //[MetaData("android.support.PARENT_ACTIVITY", Value = "criminalintent.CrimeListActivity")]  // 注意项目名小写
    public class CrimePagerActivity : FragmentActivity
    {
        private ViewPager viewPager;
        private JavaList<Crime> crimes;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            viewPager = new ViewPager(this);
            viewPager.Id = Resource.Id.viewPager;
            SetContentView(viewPager);

            crimes = CrimeLab.Get(this).Crimes;
            viewPager.Adapter = new CustomFragmentStatePagerAdapter(SupportFragmentManager, crimes);
            viewPager.PageSelected += (s, e) =>
            {
                //ActionBar.Title = crimes[e.Position].Title;
                Title = crimes[e.Position].Title;
            };

            var id = (UUID)Intent.GetSerializableExtra(CrimeFragment.EXTRA_CRIME_ID);
            Crime crime = crimes.First(c => c.Id.Equals(id));
            Title = crime.Title;

            // 用于正确显示第一次加载 ViewPager 中的项
            var index = new List<Crime>(crimes).FindIndex(c => c.Id.Equals(id));
            //var index = crimes.FindIndex(c => c.Id.Equals(id));
            viewPager.SetCurrentItem(index, true);  // 此处会将 index 传入 position
        }
    }

    // 自定义类用于满足Java中 FragmentStatePagerAdapter 的使用
    class CustomFragmentStatePagerAdapter : FragmentStatePagerAdapter
    {
        private JavaList<Crime> crimes;

        public CustomFragmentStatePagerAdapter(Android.Support.V4.App.FragmentManager fm, JavaList<Crime> crimes)
            : base(fm)
        {
            this.crimes = crimes;
        }

        public override int Count
        {
            get { return crimes.Count; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            Crime c = crimes[position];
            return CrimeFragment.NewInstance(c.Id);
        }
    }
}