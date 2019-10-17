using Foundation;
using GodAndMe.DependencyServices;
using System;
using UIKit;
using Xamarin.Forms;

namespace GodAndMe.iOS
{
    public partial class UnlockViewController : UIViewController
    {
        public UnlockViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        partial void UseTouchID_Tapped(UIButton sender)
        {
            App.justUnlocked = false;
            App.justShowedUnlockView = false;
            App.AuthenticatedWithTouchID();
        }
    }
}