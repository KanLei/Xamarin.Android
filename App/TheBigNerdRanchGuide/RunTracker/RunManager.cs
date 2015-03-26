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
using Android.Locations;

namespace RunTracker
{
    public class RunManager
    {
        private const string TAG = "RunManager";
        public const string ACTION_LOCATION = "RunTracker.ACTION_LOCATION";

        private static RunManager runManager;
        private Context appContext;
        private LocationManager locationManager;

        // The private constructor forces users to use RunManager.Get(Context)
        private RunManager(Context appContext)
        {
            this.appContext = appContext;
            locationManager = (LocationManager)appContext.GetSystemService(Context.LocationService);
        }

        public static RunManager Get(Context c)
        {
            if (runManager == null)
            {
                // Use the application context to avoid leaking activities
                runManager = new RunManager(c.ApplicationContext);
            }
            return runManager;
        }

        private PendingIntent GetLocationPendingIntent(bool shouldCreate)
        {
            var broadcast = new Intent(ACTION_LOCATION);
            int flags = shouldCreate ? 0 : (int)PendingIntentFlags.NoCreate;
            return PendingIntent.GetBroadcast(appContext, 0, broadcast, (PendingIntentFlags)flags);
        }

        public void StartLocationUpdates()
        {
            string provider = LocationManager.GpsProvider;

            // Start updates from the location manager
            PendingIntent pi = GetLocationPendingIntent(true);
            locationManager.RequestLocationUpdates(provider, 0, 0, pi);
        }

        public void StopLocationUpdates()
        {
            PendingIntent pi = GetLocationPendingIntent(false);
            if(pi!=null)
            {
                locationManager.RemoveUpdates(pi);
                pi.Cancel();
            }
        }

        public bool IsTrackingRun()
        {
            return GetLocationPendingIntent(false) != null;
        }
    }
}