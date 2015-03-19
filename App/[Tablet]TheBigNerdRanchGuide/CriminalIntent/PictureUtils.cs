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
using Android.Graphics.Drawables;
using Android.Graphics;
using System.Threading.Tasks;
using System.IO;

namespace CriminalIntent
{
    class PictureUtils
    {
        // 缩放图片大小
        public static BitmapDrawable GetScaledDrawable(Activity activity, string path, bool scale = true)
        {
            Display display = activity.WindowManager.DefaultDisplay;
            float destWidth = display.Width;
            float destHeight = display.Height;

            // Read in the dimension of the image on disk
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;  // 这句设置为 true, 下面这行代码不返回 bitmap 实例, 只是设置 options 参数
            BitmapFactory.DecodeFile(path, options);
            // 获取原始图片尺寸
            float srcWidth = options.OutWidth;
            float srcHeight = options.OutHeight;

            int inSampleSize = 1;
            if (srcHeight > destHeight || srcWidth > destWidth)
            {
                if (srcWidth > srcHeight)
                {
                    inSampleSize = (int)(srcHeight / destHeight); // 是原始图像的几倍，就缩小几倍
                }
                else
                {
                    inSampleSize = (int)(srcWidth / destWidth);
                }

            }

            options = new BitmapFactory.Options();
            options.InSampleSize = inSampleSize;

            Bitmap bitmap = BitmapFactory.DecodeFile(path, options);

            return new BitmapDrawable(activity.Resources, bitmap);
        }

        // 释放图片资源
        public static void CleanImageView(ImageView imageView)
        {
            // Clean up the view's image for the sake of memory
            BitmapDrawable b = imageView.Drawable as BitmapDrawable;
            if (b != null)
            {
                b.Bitmap.Recycle();
                imageView.SetImageDrawable(null);
            }
        }

        // 删除图片文件
        public static void DeleteImageFromFile(string path)
        {
            File.Delete(path);
        }
    }
}