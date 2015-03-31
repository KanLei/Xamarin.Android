using System;
using System.Collections.Generic;

namespace Feeder
{
	public class RssFeedLab
	{
		private static RssFeedLab rssFeedLab;
		public List<RssFeed> RssFeeds {
			get;
			set;
		}

		private RssFeedLab ()
		{
			RssFeeds = new List<RssFeed> ();
		}

		public static RssFeedLab Get()
		{
			if(rssFeedLab == null)
				rssFeedLab = new RssFeedLab ();
			return rssFeedLab;
		}
	}
}

