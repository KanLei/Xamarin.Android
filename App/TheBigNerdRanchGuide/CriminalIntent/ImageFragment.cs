using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using System.Threading.Tasks;

namespace CriminalIntent
{
    public class ImageFragment : DialogFragment
    {
        public const string EXTRA_IMAGE_PATH = "CriminalIntent.ImageFragment.ImagePath";

        private ImageView imageView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            imageView = new ImageView(Activity);
//            var path = ;
//			BitmapDrawable image;
//			var task = await PictureUtils.GetScaledDrawableAsync(Activity, Arguments.GetString(EXTRA_IMAGE_PATH), false).ConfigureAwait(false));
//			imageView.SetImageDrawable (task.Result);
////            imageView.SetImageDrawable(image);
//            return imageView;

			imageView.SetImageDrawable (GetBitmap ());
			return imageView;
        }

		private BitmapDrawable GetBitmap()
		{
			Task<BitmapDrawable> task = PictureUtils.GetScaledDrawableAsync(Activity, Arguments.GetString(EXTRA_IMAGE_PATH), false);
			return task.Result;
		}

        public override void OnDestroy()
        {
            base.OnDestroy();

            PictureUtils.CleanImageView(imageView);
        }

        public static ImageFragment NewInstance(string imagePath)
        {
            var args = new Bundle();
            args.PutString(EXTRA_IMAGE_PATH, imagePath);

            var imageFragment = new ImageFragment();
            imageFragment.Arguments = args;
            imageFragment.SetStyle(DialogFragmentStyle.NoTitle, 0);

            return imageFragment;
        }
    }
}