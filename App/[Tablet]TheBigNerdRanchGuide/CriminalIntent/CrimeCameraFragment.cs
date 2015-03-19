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
using Android.Hardware;
using System.IO;

namespace CriminalIntent
{
    public class CrimeCameraFragment : Android.Support.V4.App.Fragment, ISurfaceHolderCallback
    {
        private const string TAG = "CrimeCameraFragment";
        public const string EXTRA_PHOTO_FILENAME = "CriminalIntent.CrimeCameraFragment.PhotoFileName";

        private Camera camera;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.fragment_crime_camera, container, false);

            var progressContainer = v.FindViewById<FrameLayout>(Resource.Id.crime_camera_progressContainer);
            progressContainer.Visibility = ViewStates.Invisible;

            var takePictureButton = v.FindViewById<Button>(Resource.Id.crime_camera_takePictureButton);
            takePictureButton.Click += (s, e) =>
            {
                if (camera != null)
                    camera.TakePicture(new MyShuttlerCallBack(progressContainer), null, new MyPictureCallback(Activity));
            };

            var surfaceView = v.FindViewById<SurfaceView>(Resource.Id.crime_camera_surfaceView);
            ISurfaceHolder holder = surfaceView.Holder;
            // This is deprecated, but are required for Camera preview to work on pre-3.0 devices.
            holder.SetType(SurfaceType.PushBuffers);
            holder.AddCallback(this);  // 注意这句要加
            return v;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Gingerbread)
                camera = Camera.Open(0);
            else
                camera = Camera.Open();
        }

        public override void OnPause()
        {
            base.OnPause();

            if (camera != null)
            {
                camera.Release();
                camera = null;
            }
        }

        #region ISurfaceHolderCallback
        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            if (camera == null) return;

            // The surface has changed size; update the camera preview size
            Camera.Parameters parameters = camera.GetParameters();
            Camera.Size s = parameters.SupportedPreviewSizes.OrderByDescending(p => p.Width * p.Height).First();
            parameters.SetPreviewSize(s.Width, s.Height);
            s = parameters.SupportedPictureSizes.OrderByDescending(p => p.Width * p.Height).First();
            parameters.SetPictureSize(s.Width, s.Height);
            camera.SetParameters(parameters);
            try
            {
                camera.StartPreview();
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, "Could not start preview", ex);
                camera.Release();
                camera = null;
            }
        }


        // Tell the camera to use this surface as its preview area
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (camera != null)
            {
                camera.SetPreviewDisplay(holder);
            }
        }

        // We can no longer display on this surface, so stop the preview.
        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            if (camera != null)
                camera.StopPreview();
        }
        #endregion


        #region Camera Callback
        private class MyShuttlerCallBack : Java.Lang.Object, Camera.IShutterCallback
        {
            private FrameLayout progressBarContainer;
            public MyShuttlerCallBack(FrameLayout progressBarContainer)
            {
                this.progressBarContainer = progressBarContainer;
            }

            public void OnShutter()
            {
                progressBarContainer.Visibility = ViewStates.Visible;
            }
        }

        private class MyPictureCallback : Java.Lang.Object, Camera.IPictureCallback
        {
            private Activity activity;
            public MyPictureCallback(Activity activity)
            {
                this.activity = activity;
            }

            public async void OnPictureTaken(byte[] data, Camera camera)
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                try
                {
                    using (Stream stream = activity.OpenFileOutput(fileName, FileCreationMode.Private))
                        await stream.WriteAsync(data, 0, data.Length);
                    var i = new Intent();
                    i.PutExtra(EXTRA_PHOTO_FILENAME, fileName);
                    activity.SetResult(Result.Ok, i);
                }
                catch (Exception)
                {
                    activity.SetResult(Result.Canceled);
                    throw;
                }
                activity.Finish();
            }
        }

        #endregion
    }
}