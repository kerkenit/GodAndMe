// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace GodAndMe.iOS
{
    [Register ("UnlockViewController")]
    partial class UnlockViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView UnlockView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton UseTouchID { get; set; }

        [Action ("UseTouchID_Tapped:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UseTouchID_Tapped (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (UnlockView != null) {
                UnlockView.Dispose ();
                UnlockView = null;
            }

            if (UseTouchID != null) {
                UseTouchID.Dispose ();
                UseTouchID = null;
            }
        }
    }
}