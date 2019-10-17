using System.Linq;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(GodAndMe.iOS.Renderers.CustomContentPageRenderer))]
namespace GodAndMe.iOS.Renderers
{
	class CustomContentPageRenderer : PageRenderer
	{
		NSObject observerHideKeyboard;
		NSObject observerShowKeyboard;
		public new ContentPage Element
		{
			get { return (ContentPage)base.Element; }
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			//App.SetTheme();
			//observerHideKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			//observerShowKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);

			ConfigureToolbarItems();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			//NSNotificationCenter.DefaultCenter.RemoveObserver(observerHideKeyboard);
			//NSNotificationCenter.DefaultCenter.RemoveObserver(observerShowKeyboard);
		}

		void OnKeyboardNotification(NSNotification notification)
		{
			if (!IsViewLoaded) return;

			CoreGraphics.CGRect frameBegin = UIKeyboard.FrameBeginFromNotification(notification);
			CoreGraphics.CGRect frameEnd = UIKeyboard.FrameEndFromNotification(notification);

			//ContentPage page = Element as ContentPage;
			//if (page != null && !(page.Content is ScrollView))
			//{
			//    Thickness padding = page.Padding;
			//    page.Padding = new Thickness(padding.Left, padding.Top, padding.Right, padding.Bottom + frameBegin.Top - frameEnd.Top);
			//}
			//else
			//{
			Rectangle bounds = Element.Bounds;
			Rectangle newBounds = new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height - frameBegin.Top + frameEnd.Top);
			Element.Layout(newBounds);
			//}
		}

		private void ConfigureToolbarItems()
		{
			if (NavigationController != null)
			{
				UINavigationItem navigationItem = NavigationController.TopViewController.NavigationItem;
				IOrderedEnumerable<ToolbarItem> orderedItems = Element.ToolbarItems.OrderBy(x => x.Priority);

				// add right side items
				UIBarButtonItem[] rightItems = orderedItems.Where(x => x.Priority >= 0).Select(x => x.ToUIBarButtonItem()).ToArray();
				navigationItem.SetRightBarButtonItems(rightItems, false);

				// add left side items, keep any already there
				UIBarButtonItem[] leftItems = orderedItems.Where(x => x.Priority < 0).Select(x => x.ToUIBarButtonItem()).ToArray();
				if (navigationItem.LeftBarButtonItems != null)
				{
					//   leftItems = navigationItem.LeftBarButtonItems.Union(leftItems).ToArray();
				}
				navigationItem.SetLeftBarButtonItems(leftItems, false);
			}
		}
	}
}