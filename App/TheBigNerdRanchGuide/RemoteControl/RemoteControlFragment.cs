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

namespace RemoteControl
{
    public class RemoteControlFragment : Fragment
    {

        private TextView selectedTextView;
        private TextView workingTextView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.fragment_remote_control, container, false);
            selectedTextView = v.FindViewById<TextView>(Resource.Id.fragment_remote_control_selectedTextView);

            workingTextView = v.FindViewById<TextView>(Resource.Id.fragment_remote_control_workingTextView);

            var tableLayout = v.FindViewById<TableLayout>(Resource.Id.fragment_remote_control_tableLayout);
            int number = 1;
            for (int i = 2; i < tableLayout.ChildCount - 1; i++)
            {
                var tableRow = tableLayout.GetChildAt(i) as TableRow;
                for (int j = 0; j < tableRow.ChildCount; j++)
                {
                    var button = tableRow.GetChildAt(j) as Button;
                    button.Text = number.ToString();
                    button.Click += NumberButton_Click;
                    number++;
                }
            }

            var deleteButton = v.FindViewById<Button>(Resource.Id.deleteButton);
            deleteButton.Text = "Delete";
            deleteButton.Click += (s, e) =>
            {
                workingTextView.Text = "0";
            };

            var zeroButton = v.FindViewById<Button>(Resource.Id.zeroButton);
            zeroButton.Text = "0";
            zeroButton.Click += NumberButton_Click;

            var enterButton = v.FindViewById<Button>(Resource.Id.enterButton);
            enterButton.Text = "Enter";
            enterButton.Click += (s, e) =>
            {
                char[] working = workingTextView.Text.ToCharArray();
                if (working.Length > 0)
                    selectedTextView.Text = new string(working);
                workingTextView.Text = "0";
            };

            return v;
        }

        void NumberButton_Click(object sender, EventArgs e)
        {
            var textView = sender as TextView;
            if (textView == null) return;

            string text = textView.Text;
            string working = workingTextView.Text;

            if (working.Equals("0"))
                workingTextView.Text = text;
            else
                workingTextView.Text = working + text;
        }
    }
}