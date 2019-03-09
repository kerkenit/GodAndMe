using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using System.Reflection;
using UIKit;
using System;
using CoreGraphics;

[assembly: ExportRenderer(typeof(ViewCell), typeof(GodAndMe.iOS.Renderers.CustomViewCellRenderer))]
namespace GodAndMe.iOS.Renderers
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            UITableViewCell cell = base.GetCell(item, reusableCell, tv);

            try
            {
                // This is the assembly full name which may vary by the Xamarin.Forms version installed.
                // NullReferenceException is raised if the full name is not correct.
                Type globalContextViewCell = Type.GetType("Xamarin.Forms.Platform.iOS.ContextActionsCell, Xamarin.Forms.Platform.iOS, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null");

                // Now change the static field value! "NormalBackground" OR "DestructiveBackground"
                if (globalContextViewCell != null)
                {
                    FieldInfo normalButton = globalContextViewCell.GetField("NormalBackground", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

                    if (normalButton != null)
                    {
                        // normalButton.SetValue(null, getImageBasedOnColor("ff9500"));
                    }

                    FieldInfo destructiveButton = globalContextViewCell.GetField("DestructiveBackground", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);


                    if (destructiveButton != null)
                    {
                        //destructiveButton.SetValue(null, getImageBasedOnColor("B3B3B3"));
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error in setting background color of Menu Item : " + e.ToString());
            }

            return cell;
        }

        private UIImage getImageBasedOnColor(string colorCode)
        {
            // Get UIImage with a green color fill
            CGRect rect = new CGRect(0, 0, 1, 1);
            CGSize size = rect.Size;
            UIGraphics.BeginImageContext(size);
            CGContext currentContext = UIGraphics.GetCurrentContext();
            currentContext.SetFillColor(Color.FromHex(colorCode).ToCGColor());
            currentContext.FillRect(rect);
            UIImage backgroundImage = UIGraphics.GetImageFromCurrentImageContext();
            currentContext.Dispose();
            return backgroundImage;
        }
    }
}