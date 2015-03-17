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

namespace HelloMoonVideo
{
    class VideoPlayer
    {
        private VideoView videoView;

        public VideoPlayer(VideoView videoView, Android.Net.Uri uri)
        {
            this.videoView = videoView;
            videoView.SetVideoURI(uri);
        }

        public void Play()
        {
            videoView.Start();
        }

        public void Pause()
        {
            videoView.Pause();
        }

        public void Stop()
        {
            videoView.StopPlayback();
        }
    }
}