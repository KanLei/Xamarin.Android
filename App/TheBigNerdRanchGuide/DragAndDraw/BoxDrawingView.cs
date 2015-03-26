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
using Android.Util;
using Android.Graphics;
using Java.Util;
using Newtonsoft.Json;

namespace DragAndDraw
{
    public class BoxDrawingView : View
    {
        public const string TAG = "BoxDrawingView";

        private Box currentBox;
        private List<Box> boxes;
        private Paint boxPaint;
        private Paint backgroundPaint;

        // Used when creating the view in code
        public BoxDrawingView(Context context)
            : this(context, null)
        {

        }

        // Used when inflating the view from XML
        public BoxDrawingView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            boxes = new List<Box>();

            // Paint the boxes a nice semitransparent color(ARGB)
            boxPaint = new Paint();
            boxPaint.Color = Color.LightSkyBlue;

            // Paint the background off-white
            backgroundPaint = new Paint();
            backgroundPaint.Color = Color.LightBlue;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            PointF curr = new PointF(e.GetX(), e.GetY());

            Log.Info(TAG, String.Format("received event at x={0}, y={1}", curr.X, curr.Y));

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    Log.Info(TAG, "Down");
                    // Reset drawing state
                    currentBox = new Box(curr);
                    boxes.Add(currentBox);
                    break;
                case MotionEventActions.Move:
                    Log.Info(TAG, "Move");
                    if (currentBox != null)
                    {
                        currentBox.Current = curr;
                        Invalidate();
                    }
                    break;
                case MotionEventActions.Up:
                    Log.Info(TAG, "Up");
                    currentBox = null;
                    break;
                case MotionEventActions.Cancel:
                    Log.Info(TAG, "Cancel");
                    currentBox = null;
                    break;
                default:
                    break;
            }
            return true;
        }

        protected override void OnDraw(Canvas canvas)
        {
            // Fill the background
            canvas.DrawPaint(backgroundPaint);

            foreach (var box in boxes)
            {
                float left = Math.Min(box.Origin.X, box.Current.X);
                float right = Math.Max(box.Origin.X, box.Current.X);
                float top = Math.Min(box.Origin.Y, box.Current.Y);
                float bottom = Math.Max(box.Origin.Y, box.Current.Y);

                canvas.DrawRect(left, top, right, bottom, boxPaint);
            }
        }

        protected override IParcelable OnSaveInstanceState()
        {
            Bundle bundle = new Bundle();
            bundle.PutParcelable("instanceState", base.OnSaveInstanceState());
            string boxesAsString = JsonConvert.SerializeObject(boxes,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            bundle.PutString(TAG, boxesAsString);

            return bundle;
        }

        protected override void OnRestoreInstanceState(IParcelable state)
        {
            Bundle bundle = state as Bundle;
            if (bundle != null)
            {
                base.OnRestoreInstanceState(bundle.GetParcelable("instanceState") as IParcelable);
                string boxesAsString = bundle.GetString(TAG);
                boxes = JsonConvert.DeserializeObject<List<Box>>(boxesAsString);
            }
            else
                base.OnRestoreInstanceState(state);
        }
    }
}