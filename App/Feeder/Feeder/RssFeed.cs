using System;
using System.Collections.Generic;


namespace Feeder
{
	public class RssFeed
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public DateTime AddDateTime { get; set; }

		public List<RssItem> Items { get; set; }

		public RssFeed (string name, string url)
		{
			this.Name = name;
			this.Url = url;
			AddDateTime = DateTime.Now;
			Items = new List<RssItem> ();
		}
	}
}

