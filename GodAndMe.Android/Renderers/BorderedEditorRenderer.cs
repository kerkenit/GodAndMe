using GodAndMe.Extensions;
using GodAndMe.Android.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Graphics;

[assembly: ExportRenderer(typeof(BorderedEditor), typeof(BorderedEditorRenderer))]
namespace GodAndMe.Android.Renderers
{
    public class BorderedEditorRenderer : EditorRenderer
    {
        public BorderedEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control == null || Element == null || e.OldElement != null) return;

            var customColor = Xamarin.Forms.Color.FromHex("#0F9D58");
            Control.Background.SetColorFilter(customColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
        }
    }
}
