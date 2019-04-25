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
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && this.Control != null)
            {
                this.AddClearButton();

                this.Control.BorderStyle = UITextBorderStyle.Line;
                Control.Layer.BorderColor = UIColor.LightGray.CGColor;
                Control.Layer.BorderWidth = 1;

                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    this.Control.Font = UIFont.SystemFontOfSize(25);
                }
            }

        }

        private void AddClearButton()
        {
            var originalToolbar = this.Control.InputAccessoryView as UIToolbar;

            if (originalToolbar != null && originalToolbar.Items.Length <= 2)
            {
                var clearButton = new UIBarButtonItem(CommonFunctions.i18n("Clear"), UIBarButtonItemStyle.Plain, ((sender, ev) =>
                {
                    GodAndMe.NullableDatePicker baseDatePicker = this.Element as GodAndMe.NullableDatePicker;
                    this.Element.Unfocus();
                    this.Element.Date = DateTime.Now;
                    baseDatePicker.CleanDate();

                }));

                var newItems = new List<UIBarButtonItem>();
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