
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
using Android.Webkit;

namespace Feeder
{
	public class PostPageFragment : Fragment
	{
		public const string WEBVIEW_URL = "PostPageFragment.WebView_Url";

		private string urlLink;

		public ProgressBar progressBar;

		public static PostPageFragment NewInstance(string link)
		{
			var postPageFragment = new PostPageFragment ();
			var args = new Bundle ();
			args.PutString (WEBVIEW_URL, link);
			postPageFragment.Arguments = args;
			return postPageFragment;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			Activity.ActionBar.Hide ();

			urlLink = Arguments.GetString (WEBVIEW_URL);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Resource.Layout.fragment_post_page, container, false);

			progressBar = v.FindViewById<ProgressBar> (Resource.Id.progressBar);
			progressBar.Max = 100;	// 0-100

			var webView = v.FindViewById<WebView> (Resource.Id.webView);
			webView.Settings.JavaScriptEnabled = true;

			webView.SetWebViewClient (new MyWebViewClient ());
			webView.SetWebChromeClient (new MyWebChromeClient(this, Activity));


			webView.LoadUrl (urlLink);

			return v;
		}


		private class MyWebViewClient:WebViewClient
		{
			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				return false;  // 返回 false, 让 WebView 加载链接
			}
		}

		private class MyWebChromeClient:WebChromeClient
		{
			private PostPageFragment mPostPageFragment;
			private Activity mActivity;

			public MyWebChromeClient(PostPageFragment postPageFragment, Activity activity)
			{
				mPostPageFragment = postPageFragment;
				mActivity = activity;
			}

			public override void OnProgressChanged (WebView view, int newProgress)
			{
				if (newProgress == 100) {
					mPostPageFragment.progressBar.Visibility = ViewStates.Gone;
				} else {
					mPostPageFragment.progressBar.Progress = newProgress;
				}
			}

//			public override void OnReceivedTitle (WebView view, string title)
//			{
//				mActivity.ActionBar.Title = title;
//			}
		}
	}
}

