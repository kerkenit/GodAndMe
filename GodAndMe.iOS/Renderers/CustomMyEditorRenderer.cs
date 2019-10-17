using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using GodAndMe.iOS.Renderers;

[assembly: ExportRenderer(typeof(MyEditor), typeof(GodAndMe.iOS.Renderers.MyEditorRenderer))]
namespace GodAndMe.iOS.Renderers
{
	public class MyEditorRenderer : EditorRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.Layer.BorderColor = UIColor.LightGray.CGColor;
				Control.Layer.BorderWidth = 1;
				Control.Layer.BackgroundColor = UIColor.White.CGColor;
			}
		}
	}
}