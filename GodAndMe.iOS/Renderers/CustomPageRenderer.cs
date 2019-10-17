using Foundation;
using GodAndMe.Controls;
using GodAndMe.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(KeyboardResizingAwareContentPage), typeof(GodAndMe.iOS.Renderers.IosKeyboardFixPageRenderer))]

namespace GodAndMe.iOS.Renderers
{
	public class IosKeyboardFixPageRenderer : PageRenderer
	{
		NSObject observerHideKeyboard;
		NSObject observerShowKeyboard;

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			observerHideKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			observerShowKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			NSNotificationCenter.DefaultCenter.RemoveObserver(observerHideKeyboard);
			NSNotificationCenter.DefaultCenter.RemoveObserver(observerShowKeyboard);
		}

		void OnKeyboardNotification(NSNotification notification)
		{
			if (!IsViewLoaded) return;

			var frameBegin = UIKeyboard.FrameBeginFromNotification(notification);
			var frameEnd = UIKeyboard.FrameEndFromNotification(notification);

			var page = Element as ContentPage;
			if (page != null && !(page.Content is ScrollView))
			{
				var padding = page.Padding;
				page.Padding = new Thickness(padding.Left, padding.Top, padding.Right, padding.Bottom + frameBegin.Top - frameEnd.Top);
			}
		}
	}
}