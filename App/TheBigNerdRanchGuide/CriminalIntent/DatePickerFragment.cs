using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.App;
using Java.Util;


/*
 * 使用构造函数构造灵活的 DatePickerFragment，代替书中案例实现
 * 使用 DatePickerDialog 代替自定义 DatePicker
 * 由于在转向时 DatePicker 会自动保留日期选择状态，因此不再使用 Arguments
 * 
 */
namespace CriminalIntent
{
    public class DatePickerFragment : Android.Support.V4.App.DialogFragment
    {
        public const string EXTRA_DATE = "TheBigNerdRanchGuid.CriminalIntent.Date";

        private Date date;


        /// <summary>
        /// 此方法用构造函数替换
        /// </summary>
        //public static DatePickerFragment newInstance(Date date)
        //{
        //    Bundle args = new Bundle();
        //    args.PutSerializable(EXTRA_DATE, date);

        //    DatePickerFragment fragment = new DatePickerFragment();
        //    fragment.Arguments = args;

        //    return fragment;
        //}

        /// <summary>
        /// 默认构造函数转向时调用
        /// </summary>
        public DatePickerFragment() { }

        // 尝试替代 NewInstance(Date date)
        public DatePickerFragment(Date date)
        {
            this.date = date;
            //var args = new Bundle();
            //args.PutSerializable(EXTRA_DATE, date);
            //this.Arguments = args;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override Android.App.Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Calendar calendar = Calendar.Instance;
            //calendar.Time = Arguments.GetSerializable(EXTRA_DATE) as Date;
            if (date != null)  // 检查转向
                calendar.Time = date;
            int year = calendar.Get(CalendarField.Year);
            int month = calendar.Get(CalendarField.Month);
            int day = calendar.Get(CalendarField.DayOfMonth);
            int hour = calendar.Get(CalendarField.HourOfDay);
            int minute = calendar.Get(CalendarField.Minute);

            //View v = Activity.LayoutInflater.Inflate(Resource.Layout.dialog_date, null);
            //var datePicker = v.FindViewById<DatePicker>(Resource.Id.dialog_date_dataPicker);
            //datePicker.DateTime = new DateTime(year, month, day);

            //return new AlertDialog.Builder(Activity)
            //    .SetView(v)
            //    .SetTitle(Resource.String.date_picker_title)
            //    .SetPositiveButton(Android.Resource.String.Ok, delegate { })
            //    .Create();

            AlertDialog dateTimeDialog;
            if (TargetRequestCode == 0)
                dateTimeDialog = new DatePickerDialog(Activity, CallBack, year, month, day);
            else
                dateTimeDialog = new TimePickerDialog(Activity, TimeCallBack, hour, minute, false);
            return dateTimeDialog;
        }

        // 点击确定回调
        private void CallBack(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            var date = new GregorianCalendar(e.Date.Year, e.Date.Month - 1, e.Date.Day).Time;
            SetResultCallBack(date);
        }

        private void TimeCallBack(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            var date = new GregorianCalendar(0, 0, 0, e.HourOfDay, e.Minute).Time;
            SetResultCallBack(date);
        }

        private void SetResultCallBack(Date date)
        {
            var i = new Intent();
            i.PutExtra(EXTRA_DATE, date);
            TargetFragment.OnActivityResult(TargetRequestCode, (int)Result.Ok, i);
        }
    }
}