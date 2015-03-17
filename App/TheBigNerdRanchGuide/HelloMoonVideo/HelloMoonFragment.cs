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

namespace HelloMoonVideo
{
    public class HelloMoonFragment : Fragment
    {
        private VideoPlayer videoPlayer;

        private Button playButton;
        private Button pauseButton;
        private Button stopButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.fragment_hello_moon, container, false);

            VideoView videoView = v.FindViewById<VideoView>(Resource.Id.hellomoon_videoView);
            videoView.SetMediaController(new MediaController(Activity));
            videoPlayer = new VideoPlayer(videoView, Android.Net.Uri.Parse("android.resource://" + Activity.PackageName + "/" + Resource.Raw.Bq6KXBAIAAAZsc0));

            playButton = v.FindViewById<Button>(Resource.Id.hellomoon_playButton);
            playButton.Click += delegate
            {
                videoPlayer.Play();
            };

            pauseButton = v.FindViewById<Button>(Resource.Id.hellomoon_pauseButton);
            pauseButton.Click += delegate
            {
                videoPlayer.Pause();
            };

            stopButton = v.FindViewById<Button>(Resource.Id.hellomoon_stopButton);
            stopButton.Click += delegate
            {
                videoPlayer.Stop();
            };

            return v;
        }
    }
}