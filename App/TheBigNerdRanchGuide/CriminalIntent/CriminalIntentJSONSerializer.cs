using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Threading.Tasks;


namespace CriminalIntent
{
    /// <summary>
    /// 用于持久化数据
    /// </summary>
    class CriminalIntentJSONSerializer
    {
        private Context context;
        private string fileName;

        public CriminalIntentJSONSerializer(Context context, string fileName)
        {
            this.context = context;
            this.fileName = fileName;
        }

        public async Task SaveToFileAsync(JavaList<Crime> crimes)
        {
            Stream stream = context.OpenFileOutput(fileName, FileCreationMode.Private);
            using (var streamWriter = new StreamWriter(stream))
            {
                string result = JsonConvert.SerializeObject(crimes,
                    Formatting.None,
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                await streamWriter.WriteAsync(result);
            }
        }

        public JavaList<Crime> ReadFromFile()
        {
            Java.IO.File file = context.GetFileStreamPath(fileName);
            if (file.Exists())
            {
                Stream stream = context.OpenFileInput(fileName);
                using (var streamReader = new StreamReader(stream))
                {
                    string result = streamReader.ReadToEnd();
					return JsonConvert.DeserializeObject<JavaList<Crime>> (result) ?? new JavaList<Crime> ();
                }
            }
            return new JavaList<Crime>();
        }
    }
}