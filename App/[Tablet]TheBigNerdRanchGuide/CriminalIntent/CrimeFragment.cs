using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Util;
using Android.Content.PM;
using Android.Hardware;
using Android.Graphics.Drawables;
using Android.Text.Format;
using Android.Provider;
using Android.Database;

namespace CriminalIntent
{
    public class CrimeFragment : Android.Support.V4.App.Fragment
    {
        public const string EXTRA_CRIME_ID = "CriminalIntent.Crime_ID";
        public const string DIALOG_DATE = "dialog_date";
        public const string DIALOG_TIME = "dialog_time";

        private const string DIALOG_IMAGE = "image";

        private const int REQUEST_DATE = 0;
        private const int REQUEST_TIME = 1;
        private const int REQUEST_PHOTO = 2;
        private const int REQUEST_CONTACT = 3;

        private Crime crime;

        private EditText titleEditText;
        private Button dateButton;
        private CheckBox solvedCheckBox;
        private ImageButton photoButton;
        private ImageView photoView;
        private Button suspectButton;

        private ICallbacks callbacks;

        /*
         * Required interface for hosting activities.
         */
        public interface ICallbacks
        {
            void OnCrimeUpdated(Crime crime);
        }


        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            callbacks = (ICallbacks)activity;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            callbacks = null;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;  // 2
            //crime = new Crime();
            // 从 Activity 获取 Intent
            //var crimeId = (UUID)Activity.Intent.GetSerializableExtra(EXTRA_CRIME_ID);
            var crimeId = (UUID)Arguments.GetSerializable(EXTRA_CRIME_ID);
            crime = CrimeLab.Get(Activity).GetCrime(crimeId);

            Activity.Title = crime.Title;
            RetainInstance = true;  // 当把手机转向时，不会再调用 OnCreate，直接调用 OnCreateView
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.fragment_crime, container, false);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
                Activity.ActionBar.SetDisplayHomeAsUpEnabled(true);  // 1

            titleEditText = v.FindViewById<EditText>(Resource.Id.crime_title);
            titleEditText.Text = crime.Title;
            titleEditText.TextChanged += (s, e) =>
            {
                crime.Title = e.Text.ToString();
                callbacks.OnCrimeUpdated(crime);
                Activity.Title = crime.Title;
            };

            dateButton = v.FindViewById<Button>(Resource.Id.crime_date);
            //dateButton.Enabled = false;
            UpdateDate();
            dateButton.Click += (s, e) =>
            {
                int i = 0;
                new AlertDialog.Builder(Activity)
                    .SetTitle("Change Date or Time?")
                    .SetSingleChoiceItems(new string[] { "Date", "Time" }, 0, (sender, args) =>
                    {
                        i = args.Which;
                    })
                    .SetPositiveButton(Android.Resource.String.Ok, delegate { ShowDataPickerFragment(i); })
                    .SetNegativeButton(Android.Resource.String.Cancel, delegate { })
                    .Create().Show();

                //var datePicker = new DatePickerFragment(crime.Date);
                //datePicker.SetTargetFragment(this, REQUEST_DATE);  // 实现 Fragment 与 Fragment 之间的回传
                //datePicker.Show(Activity.SupportFragmentManager, DIALOG_DATE);
            };

            solvedCheckBox = v.FindViewById<CheckBox>(Resource.Id.crime_solved);
            solvedCheckBox.Checked = crime.Solved;
            solvedCheckBox.CheckedChange += (s, e) =>
            {
                crime.Solved = e.IsChecked;
                callbacks.OnCrimeUpdated(crime);
            };

            photoButton = v.FindViewById<ImageButton>(Resource.Id.crime_imageButton);
            photoButton.Click += (s, e) =>
            {
                var i = new Intent(Activity, typeof(CrimeCameraActivity));
                StartActivityForResult(i, REQUEST_PHOTO);
            };

            photoView = v.FindViewById<ImageView>(Resource.Id.crime_imageView);
            photoView.Click += (s, e) =>
            {
                Photo p = crime.CrimePhoto;
                if (p == null) return;

                string path = Activity.GetFileStreamPath(p.PhotoName).AbsolutePath;
                var fragment = ImageFragment.NewInstance(path);
                fragment.Show(Activity.FragmentManager, DIALOG_IMAGE);
            };

            var reportButton = v.FindViewById<Button>(Resource.Id.crime_reportButton);
            reportButton.Click += (s, e) =>
            {
                var i = new Intent(Intent.ActionSend);
                i.SetType("text/plain");
                i.PutExtra(Intent.ExtraText, GetCrimeReport());
                i.PutExtra(Intent.ExtraSubject, GetString(Resource.String.crime_report_subject));
                i = Intent.CreateChooser(i, GetString(Resource.String.send_report));
                StartActivity(i);
            };

            suspectButton = v.FindViewById<Button>(Resource.Id.crime_suspectButton);
            suspectButton.Click += (s, e) =>
            {
                var i = new Intent(Intent.ActionPick, ContactsContract.Contacts.ContentUri);
                StartActivityForResult(i, REQUEST_CONTACT);
            };

            if (crime.Suspect != null)
                suspectButton.Text = crime.Suspect;


            // If camera is not available, disable camera functionality
            PackageManager pm = Activity.PackageManager;
            bool hasACamera = pm.HasSystemFeature(PackageManager.FeatureCamera) ||
                pm.HasSystemFeature(PackageManager.FeatureCameraFront) ||
                (Build.VERSION.SdkInt >= BuildVersionCodes.Gingerbread && Camera.NumberOfCameras > 0);
            if (!hasACamera)
                photoButton.Enabled = false;

            // If activities available
            IList<ResolveInfo> resolvers = pm.QueryIntentActivities(new Intent(Intent.ActionPick, ContactsContract.Contacts.ContentUri), 0);
            suspectButton.Enabled = resolvers.Count > 0;
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            resolvers = pm.QueryIntentActivities(intent, 0);
            reportButton.Enabled = resolvers.Count > 0;
            return v;
        }

        public override void OnStart()
        {
            base.OnStart();

            ShowPhoto();
        }

        public override void OnStop()
        {
            base.OnStop();

            PictureUtils.CleanImageView(photoView);
        }

        private void ShowPhoto()
        {
            Photo p = crime.CrimePhoto;
            BitmapDrawable b = null;
            try
            {
                if (p != null)
                {
                    string path = Activity.GetFileStreamPath(p.PhotoName).AbsolutePath;
                    b = PictureUtils.GetScaledDrawable(Activity, path);  //.ConfigureAwait(false); 下面访问 photoView
                }
                photoView.SetImageDrawable(b);
            }
            catch (Exception e)
            {

                throw;
            }

        }


        public override async void OnPause()
        {
            base.OnPause();

            await CrimeLab.Get(Activity).SaveCrimesAsync().ConfigureAwait(false);
        }


        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            inflater.Inflate(Resource.Menu.menu_list_item_crime_fragment, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)  // 3
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_item_delete_crime_fragment:

                    var dialog = new AlertDialog.Builder(Activity)
                        .SetTitle("Do you want to delete this crime?")
                        .SetPositiveButton(Android.Resource.String.Ok, delegate
                        {
                            CrimeLab.Get(Activity).Remove(crime);
                            BackToCrimeListActivity();
                        })
                        .SetNegativeButton(Android.Resource.String.Cancel, delegate { })
                        .Show();

                    return true;
                case Android.Resource.Id.Home:
                    BackToCrimeListActivity();
                    //try
                    //{
                    //    if (NavUtils.ParentActivity != null)  // 检查 MeteData 中是否定义 parent activity
                    //        NavUtils.NavigateUpFromSameTask(Activity);
                    //}
                    //catch (Exception e)
                    //{
                    //    //Log.Debug("TAG", e.Message);
                    //    System.Diagnostics.Debug.WriteLine(e.ToString());
                    //    throw;
                    //}

                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void BackToCrimeListActivity()
        {
            var intent = new Intent(Activity, typeof(CrimeListActivity));
            intent.AddFlags(ActivityFlags.ClearTop);  // 在 back stack 中查找 CrimeListActivity 实例，并 pop off 其上的 activity
            StartActivity(intent);
        }

        private void ShowDataPickerFragment(int i)
        {
            var datePicker = new DatePickerFragment(crime.Date);

            datePicker.SetTargetFragment(this, i == 0 ? REQUEST_DATE : REQUEST_TIME);  // 实现 Fragment 与 Fragment 之间的回传
            datePicker.Show(Activity.SupportFragmentManager, i == 0 ? DIALOG_DATE : DIALOG_TIME);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (resultCode != (int)Result.Ok) return;

            switch (requestCode)
            {
                case REQUEST_DATE:
                    var date = (Date)data.GetSerializableExtra(DatePickerFragment.EXTRA_DATE);
                    date.Hours = crime.Date.Hours;
                    date.Minutes = crime.Date.Minutes;
                    date.Seconds = crime.Date.Seconds;
                    crime.Date = date;
                    //crime.Date = date;
                    callbacks.OnCrimeUpdated(crime);
                    break;
                case REQUEST_TIME:
                    var time = (Date)data.GetSerializableExtra(DatePickerFragment.EXTRA_DATE);
                    crime.Date.Hours = time.Hours;
                    crime.Date.Minutes = time.Minutes;
                    crime.Date.Seconds = 0;
                    callbacks.OnCrimeUpdated(crime);
                    break;
                case REQUEST_PHOTO:
                    // 删除原始图片
                    if (crime.CrimePhoto != null)
                    {
                        string path = Activity.GetFileStreamPath(crime.CrimePhoto.PhotoName).AbsolutePath;
                        PictureUtils.DeleteImageFromFile(path);
                    }

                    var photoName = data.GetStringExtra(CrimeCameraFragment.EXTRA_PHOTO_FILENAME);
                    var photo = new Photo(photoName);
                    crime.CrimePhoto = photo;
                    callbacks.OnCrimeUpdated(crime);

                    ShowPhoto();
                    break;
                case REQUEST_CONTACT:
                    Android.Net.Uri contactUri = data.Data;
                    string[] queryFields = { ContactsContract.Contacts.InterfaceConsts.DisplayName };
                    ICursor cursor = Activity.ContentResolver.Query(contactUri, queryFields, null, null, null);

                    // double-check
                    if (cursor.Count == 0)
                    {
                        cursor.Close();
                        return;
                    }

                    cursor.MoveToFirst();
                    string name = cursor.GetString(cursor.GetColumnIndex(queryFields[0]));
                    crime.Suspect = name;
                    callbacks.OnCrimeUpdated(crime);

                    suspectButton.Text = name;
                    cursor.Close();
                    break;
                default:
                    break;
            }
            UpdateDate();
        }

        private void UpdateDate()
        {
            dateButton.Text = crime.Date.ToLocaleString();
        }

        public static CrimeFragment NewInstance(UUID crimeId)
        {
            var bundle = new Bundle();
            bundle.PutSerializable(EXTRA_CRIME_ID, crimeId);

            var crimeFragment = new CrimeFragment();
            crimeFragment.Arguments = bundle;

            return crimeFragment;
        }


        private string GetCrimeReport()
        {
            string solvedString = null;
            if (crime.Solved)
                solvedString = GetString(Resource.String.crime_report_solved);
            else
                solvedString = GetString(Resource.String.crime_report_unsolved);

            string dateFormat = "EEE, MMM dd";
            string dateString = DateFormat.Format(dateFormat, crime.Date).ToString();

            string suspect = crime.Suspect;
            if (suspect == null)
                suspect = GetString(Resource.String.crime_report_no_suspect);
            else
                suspect = GetString(Resource.String.crime_report_suspect, suspect);

            string report = GetString(Resource.String.crime_report, crime.Title,
                dateString, solvedString, suspect);

            return report;
        }

    }
}