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
        // ����ͼƬ��С
        public static BitmapDrawable GetScaledDrawable(Activity activity, string path, bool scale = true)
        {
            Display display = activity.WindowManager.DefaultDisplay;
            float destWidth = display.Width;
            float destHeight = display.Height;

            // Read in the dimension of the image on disk
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;  // �������Ϊ true, �������д��벻���� bitmap ʵ��, ֻ������ options ����
            BitmapFactory.DecodeFile(path, options);
            // ��ȡԭʼͼƬ�ߴ�
            float srcWidth = options.OutWidth;
            float srcHeight = options.OutHeight;

            int inSampleSize = 1;
            if (srcHeight > destHeight || srcWidth > destWidth)
            {
                if (srcWidth > srcHeight)
                {
                    inSampleSize = (int)(srcHeight / destHeight); // ��ԭʼͼ��ļ���������С����
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

        // �ͷ�ͼƬ��Դ
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

        // ɾ��ͼƬ�ļ�
        public static void DeleteImageFromFile(string path)
        {
            File.Delete(path);
        }
    }
}