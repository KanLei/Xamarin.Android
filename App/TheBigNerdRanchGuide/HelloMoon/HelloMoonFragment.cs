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

namespace HelloMoon
{
    public class HelloMoonFragment : Fragment
    {
        private Button playButton;
        private Button pauseButton;
        private Button stopButton;

        private AudioPlayer audioPlayer = new AudioPlayer();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.fragment_hello_moon, container, false);
            playButton = v.FindViewById<Button>(Resource.Id.hellomoon_playButton);
            playButton.Click += delegate { audioPlayer.Play(Activity); };

            pauseButton = v.FindViewById<Button>(Resource.Id.hellomoon_pauseButton);
            pauseButton.Click += delegate { audioPlayer.Pause(); };

            stopButton = v.FindViewById<Button>(Resource.Id.hellomoon_stopButton);
            stopButton.Click += delegate { audioPlayer.Stop(); };

            return v;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            audioPlayer.Stop();
        }
    }
}