using System;
using System.Collections.Generic;


namespace Feeder
{
	public class RssFeed
	{
		public string Name {
			get;
			private set;
		}
		public DateTime AddDateTime {
			get;
			private set;
		}

		public List<RssItem> Items {
			get;
			set;
		}

		public RssFeed (string name)
		{
			this.Name = name;
			AddDateTime = DateTime.Now;
			Items = new List<RssItem> ();
		}
	}
}

