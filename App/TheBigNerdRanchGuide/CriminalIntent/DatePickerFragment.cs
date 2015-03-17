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
 * ʹ�ù��캯���������� DatePickerFragment���������а���ʵ��
 * ʹ�� DatePickerDialog �����Զ��� DatePicker
 * ������ת��ʱ DatePicker ���Զ���������ѡ��״̬����˲���ʹ�� Arguments
 * 
 */
namespace CriminalIntent
{
    public class DatePickerFragment : Android.Support.V4.App.DialogFragment
    {
        public const string EXTRA_DATE = "TheBigNerdRanchGuid.CriminalIntent.Date";

        private Date date;


        /// <summary>
        /// �˷����ù��캯���滻
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
        /// Ĭ�Ϲ��캯��ת��ʱ����
        /// </summary>
        public DatePickerFragment() { }

        // ������� NewInstance(Date date)
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
            if (date != null)  // ���ת��
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

        // ���ȷ���ص�
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