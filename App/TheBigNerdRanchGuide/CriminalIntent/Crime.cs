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
using Java.Util;

namespace CriminalIntent
{
	[Preserve(AllMembers=true)]
	[Serializable]
    public class Crime
    {
        public UUID Id { get; set; }
        public Date Date { get; set; }
        public string Title { get; set; }
        public bool Solved { get; set; }
        public Photo CrimePhoto { get; set; }
        public string Suspect { get; set; }

        public Crime()
        {
            // Generate unique identifier
            Id = UUID.RandomUUID();
            Date = new Date();
        }
    }
}