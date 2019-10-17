using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using System.Collections.Generic;
using GodAndMe.iOS.Renderers;

[assembly: ExportRenderer(typeof(GodAndMe.NullableDatePicker), typeof(NullableDatePickerRenderer))]
namespace GodAndMe.iOS.Renderers
{
    public class NullableDatePickerRenderer : DatePickerRenderer
    {
        //public NullableDatePickerRenderer()
        //{
        //    if (Control != null && Control.Layer != null)
        //    {
        //        Control.BorderStyle = UITextBorderStyle.RoundedRect;
        //        //Control.Layer.BorderColor = UIColor.LightGray.CGColor;
        //        Control.Layer.BorderWidth = 1;
        //    }
        //}
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control != null)
            {
                AddClearButton();

                //Control.BorderStyle = UITextBorderStyle.RoundedRect;
                //Control.Layer.BorderColor = UIColor.LightGray.CGColor;
                //Control.Layer.BorderWidth = 1;

                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    Control.Font = UIFont.SystemFontOfSize(25);
                }
            }

        }

        private void AddClearButton()
        {
            UIToolbar originalToolbar = Control.InputAccessoryView as UIToolbar;

            if (originalToolbar != null && originalToolbar.Items.Length <= 2)
            {
                var clearButton = new UIBarButtonItem(CommonFunctions.i18n("Clear"), UIBarButtonItemStyle.Plain, (sender, ev) =>
                {
                    NullableDatePicker baseDatePicker = this.Element as NullableDatePicker;
                    Element.Unfocus();
                    Element.Date = DateTime.Now;
                    baseDatePicker.CleanDate();

                });

                List<UIBarButtonItem> newItems = new List<UIBarButtonItem>();
                foreach (var item in originalToolbar.Items)
                {
                    newItems.Add(item);
                }

                newItems.Insert(0, clearButton);

                originalToolbar.Items = newItems.ToArray();
                originalToolbar.SetNeedsDisplay();
            }

        }
    }
}