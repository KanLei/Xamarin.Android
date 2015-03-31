using System;

namespace PhotoGallery
{
	public class GalleryItem
	{
		private string Caption;
		private string Id;
		private string Url;

		public GalleryItem ()
		{
		}

		public GalleryItem (string caption)
		{
			this.Caption = caption;
		}

		public override string ToString ()
		{
			return Caption;
		}
	}
}

