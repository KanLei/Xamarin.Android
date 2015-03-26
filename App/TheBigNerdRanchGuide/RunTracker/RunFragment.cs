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

namespace RunTracker
{
    public class RunFragment : Fragment
    {
        private RunManager runManager;

        private Button startButton;
        private Button stopButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            runManager = RunManager.Get(Activity);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.fragment_run, container, false);

            var startedTextView = v.FindViewById<TextView>(Resource.Id.run_startedTextView);
            var latitudeTextView = v.FindViewById<TextView>(Resource.Id.run_latitudeTextView);
            var longitudeTextView = v.FindViewById<TextView>(Resource.Id.run_longitudeTextView);
            var altitudeTextView = v.FindViewById<TextView>(Resource.Id.run_altitudeTextView);
            var durationTextView = v.FindViewById<TextView>(Resource.Id.run_durationTextView);

            startButton = v.FindViewById<Button>(Resource.Id.run_startButton);
            startButton.Click += (s, e) =>
            {
                runManager.StartLocationUpdates();
                UpdateUI();
            };
            stopButton = v.FindViewById<Button>(Resource.Id.run_stopButton);
            stopButton.Click += (s, e) =>
            {
                runManager.StopLocationUpdates();
                UpdateUI();
            };

            UpdateUI();

            return v;
        }

        private void UpdateUI()
        {
            bool started = runManager.IsTrackingRun();

            startButton.Enabled = !started;
            stopButton.Enabled = started;
        }
    }
}