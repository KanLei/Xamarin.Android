using System;

using Android.Content;
using Android.Database;

namespace Xamarin.Android
{
    public class Cursor
    {
        // using CursorLoader: on a background thread
        CursorLoader loader = new CursorLoader(activity, uri, projection, null, null, null);
        var cursor = (ICursor)loader.LoadInBackground();


        // Method 1:
        if (cursor.MoveToFirst())
        {
            do
            {
                // TODO:
            } while (cursor.MoveToNext());
        }


        // Method 2:
        for (bool flag = cursor.MoveToFirst(); flag; flag = cursor.MoveToNext())
        {
            // TODO:       
        }
    }
}