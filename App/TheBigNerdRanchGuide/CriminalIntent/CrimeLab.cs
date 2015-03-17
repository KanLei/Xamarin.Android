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
using System.Threading.Tasks;
using Android.Util;

namespace CriminalIntent
{
    public class CrimeLab
    {
        private const string FILE_NAME = "crimes.json";
        private const string TAG = "CrimeLab";

        private CriminalIntentJSONSerializer criminalIntentJson;
        public JavaList<Crime> Crimes { get; private set; }

        private static CrimeLab crimeLab;
        private Context appContext;

        private CrimeLab(Context appContext)
        {
            this.appContext = appContext;
            criminalIntentJson = new CriminalIntentJSONSerializer(appContext, FILE_NAME);

            Crimes = criminalIntentJson.ReadFromFile();

            //var query= Enumerable.Range(0, 2).Select(i => new Crime { Title = "Crime #" + i, Solved = i % 2 == 0 });
            //Crimes.AddRange(query);
        }


        public static CrimeLab Get(Context c)
        {
            if (crimeLab == null)
                crimeLab = new CrimeLab(c);
            return crimeLab;
        }


        public void AddCrime(Crime c)
        {
            Crimes.Add(c);
        }

        public void Remove(Crime c)
        {
            Crimes.Remove(c);
        }

        public Crime GetCrime(UUID id)
        {
            return Crimes.First(c => c.Id.Equals(id));
        }

        public async Task<bool> SaveCrimesAsync()
        {
            try
            {
                await criminalIntentJson.SaveToFileAsync(Crimes);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(TAG, "Error saving crimes: " + e.Message);
                return false;
            }
        }
    }
}