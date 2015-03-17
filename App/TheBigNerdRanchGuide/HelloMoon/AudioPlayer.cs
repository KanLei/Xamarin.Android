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
using Android.Media;

namespace HelloMoon
{
    class AudioPlayer
    {
        private MediaPlayer mediaPlayer;

        public void Play(Context c)
        {
            Stop();

            mediaPlayer = MediaPlayer.Create(c, Resource.Raw.one_small_step);
            mediaPlayer.Completion += delegate { Stop(); };
            mediaPlayer.Start();
        }

        public void Pause()
        {
            mediaPlayer.Pause();
        }

        public void Stop()
        {
            if(mediaPlayer!=null)
            {
                mediaPlayer.Release();
                mediaPlayer = null;
            }
        }
    }
}