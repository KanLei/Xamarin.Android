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
using Android.Graphics;

namespace DragAndDraw
{
    [Serializable]
    public class Box
    {
        public PointF Origin { get; set; }
        public PointF Current { get; set; }

        public Box(PointF origin)
        {
            Origin = Current = origin;
        }
    }
}