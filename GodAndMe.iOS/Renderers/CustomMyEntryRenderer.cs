using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using GodAndMe.iOS.Renderers;

[assembly: ExportRenderer(typeof(MyEntry), typeof(GodAndMe.iOS.Renderers.MyEntryRenderer))]
namespace GodAndMe.iOS.Renderers
{
	public class MyEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.BorderStyle = UITextBorderStyle.RoundedRect;
				Control.Layer.BorderColor = UIColor.LightGray.CGColor;
				Control.Layer.BorderWidth = 1;
				Control.Layer.BackgroundColor = UIColor.White.CGColor;
			}
		}
	}
}