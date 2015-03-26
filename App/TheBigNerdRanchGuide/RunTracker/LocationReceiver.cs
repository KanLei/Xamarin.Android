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
using Android.Util;

namespace RunTracker
{
    public class LocationReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            // If you got a location extra, use it
            Location loc = (Location)intent.GetParcelableExtra(LocationManager.KeyLocationChanged);
            if (loc != null)
            {
                OnLocationReceived(context, loc);
                return;
            }

            // If you get here, something else has happened
            if (intent.HasExtra(LocationManager.KeyProviderEnabled))
            {
                bool enabled = intent.GetBooleanExtra(LocationManager.KeyProviderEnabled, false);
                OnProviderEnabledChanged(enabled);
            }
        }

        private void OnProviderEnabledChanged(bool enabled)
        {
            Log.Debug("", String.Format("Provider {0}", enabled ? "enabled" : "disabled"));
        }

        private void OnLocationReceived(Context context, Location loc)
        {
            Log.Debug("", String.Format("{0} Get location from {1} : {2}, {3}", this, loc.Provider, loc.Latitude, loc.Longitude));
        }
    }
}