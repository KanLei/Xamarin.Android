using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CriminalIntent
{
    public class CrimeListFragment : Android.Support.V4.App.ListFragment, ActionMode.ICallback
    {
        private bool subtitileVisible;

        private ActionMode actionMode;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RetainInstance = true;
            HasOptionsMenu = true;

            Activity.SetTitle(Resource.String.crimes_title);
            // Create your fragment here
            JavaList<Crime> crimes = CrimeLab.Get(Activity).Crimes;

            this.ListAdapter = new CrimeAdapter(Activity, crimes);
        }


        public override void OnResume()
        {
            base.OnResume();

            var adapter = ListAdapter as CrimeAdapter;
            if (adapter != null)
                adapter.NotifyDataSetChanged();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = base.OnCreateView(inflater, container, savedInstanceState);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb && subtitileVisible)
            {
                Activity.ActionBar.SetSubtitle(Resource.String.subtitle);
            }

            var listView = v.FindViewById<ListView>(Android.Resource.Id.List);  // ListFragment 内置
            if (Build.VERSION.SdkInt < BuildVersionCodes.Honeycomb)
            {
                // Use floating context menus on Froyo and Gingerbread
                RegisterForContextMenu(listView);
            }
            else
            {
                // Use contextual action bar on Honeycomb and higher
                listView.ChoiceMode = ChoiceMode.Multiple;
                listView.ItemLongClick += (s, e) =>
                {
                    
                    listView.SetItemChecked(e.Position, true);
                    if (actionMode == null)
                        actionMode = Activity.StartActionMode(this);
                };
            }

            return v;
        }

        #region Option Menu
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            inflater.Inflate(Resource.Menu.menu_list_item_crime, menu);
            IMenuItem showSubtitle = menu.FindItem(Resource.Id.menu_item_show_subtitle);
            if (showSubtitle != null && subtitileVisible)
                showSubtitle.SetTitle(Resource.String.hide_subtitle);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_item_new_crime:
                    var intent = new Intent(Activity, typeof(CrimePagerActivity));
                    var crime = new Crime();
                    CrimeLab.Get(Activity).AddCrime(crime);
                    intent.PutExtra(CrimeFragment.EXTRA_CRIME_ID, crime.Id);
                    StartActivityForResult(intent, 0);
                    return true;
                case Resource.Id.menu_item_show_subtitle:
                    if (Activity.ActionBar.Subtitle == null)
                    {
                        Activity.ActionBar.SetSubtitle(Resource.String.subtitle);
                        item.SetTitle(Resource.String.hide_subtitle);
                        subtitileVisible = true;
                    }
                    else
                    {
                        Activity.ActionBar.Subtitle = null;
                        item.SetTitle(Resource.String.show_subtitle);
                        subtitileVisible = false;
                    }
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }

        }
        #endregion


        #region  Contextual Menu

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);

            Activity.MenuInflater.Inflate(Resource.Menu.menu_list_item_crime_context, menu);
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_item_delete_crime:
                    var adapterContextMenuInfo = item.MenuInfo as Android.Widget.AdapterView.AdapterContextMenuInfo;
                    var adapter = ListAdapter as CrimeAdapter;
                    if (adapter != null)
                    {
                        Crime c = adapter.GetItem(adapterContextMenuInfo.Position);
                        CrimeLab.Get(Activity).Remove(c);
                        adapter.NotifyDataSetChanged();
                    }

                    return true;
                default:
                    return base.OnContextItemSelected(item);
            }
        }

        #endregion


        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);

            if (actionMode != null) return;

            l.ClearChoices();  // ActionMode 未启用时，不标记选中状态

            var adapter = ListAdapter as CrimeAdapter;
            if (adapter != null)
            {
                Crime c = adapter.GetItem(position);
                var intent = new Intent(Activity, typeof(CrimePagerActivity));
                intent.PutExtra(CrimeFragment.EXTRA_CRIME_ID, c.Id);
                StartActivity(intent);
            }
        }

        private class CrimeAdapter : ArrayAdapter<Crime>
        {
            private Activity activity;

            public CrimeAdapter(Activity activity, JavaList<Crime> crimes)
                : base(activity, 0, crimes)
            {
                this.activity = activity;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                if (convertView == null)
                    convertView = activity.LayoutInflater.Inflate(Resource.Layout.list_item_crime, null, false);

                Crime crime = GetItem(position);
                var titleTextView = convertView.FindViewById<TextView>(Resource.Id.crime_list_item_titleTextView);
                titleTextView.Text = crime.Title;
                var dateTextView = convertView.FindViewById<TextView>(Resource.Id.crime_list_item_dateTextView);
                dateTextView.Text = crime.Date.ToLocaleString();
                var solvedCheckBox = convertView.FindViewById<CheckBox>(Resource.Id.crime_list_item_solvedCheckBox);
                solvedCheckBox.Checked = crime.Solved;

                return convertView;
            }
        }


        // ActionMode CallBack Methods
        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_item_delete_crime:
                    var adapter = ListAdapter as CrimeAdapter;
                    CrimeLab crimeLab = CrimeLab.Get(Activity);

                    for (int i = adapter.Count - 1; i >= 0; i--)
                    {
                        if (ListView.IsItemChecked(i))
                            crimeLab.Remove(adapter.GetItem(i));
                    }
                    mode.Finish();
                    //adapter.NotifyDataSetChanged();  // 在 OnDestroyActionMode 中调用
                    SaveToFile();   // 更新到文件
                    return true;
                default:
                    return false;
            }
        }

        private async void SaveToFile()
        {
            await CrimeLab.Get(Activity).SaveCrimesAsync().ConfigureAwait(false);
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            mode.MenuInflater.Inflate(Resource.Menu.menu_list_item_crime_context, menu);
            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
            ListView.ClearChoices();
            var adapter = ListAdapter as CrimeAdapter;
            if (adapter != null)
                adapter.NotifyDataSetChanged();
            actionMode = null;
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return false;
        }
    }
}