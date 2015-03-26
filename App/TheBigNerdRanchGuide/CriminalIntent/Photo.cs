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

namespace CriminalIntent
{
	[Preserve(AllMembers=true)]
	[Serializable]
    public class Photo
    {
        public string PhotoName { get; set; }  // private set 导致 JSON 反序列化后无法赋值？

		// 此构造函数与 Preserve 特性结合避免 Release 模式下 JSON 调用出错
		public Photo ()
		{
			
		}

        public Photo(string photoName)
        {
            this.PhotoName = photoName;
        }
    }
}