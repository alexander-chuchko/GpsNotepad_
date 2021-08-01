using GpsNotepad.Controls;
using GpsNotepad.iOS.Renders;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRendererIos))]
namespace GpsNotepad.iOS.Renders
{
    public class CustomEntryRendererIos:EntryRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Layer.CornerRadius = 4;
                Control.Layer.BorderWidth = 2f;
                Control.Layer.BorderColor = Color.LightGray.ToCGColor();

                Control.LeftView = new UIKit.UIView(new CGRect(0, 0, 12, 0));
                Control.LeftViewMode = UIKit.UITextFieldViewMode.Always;
            }
        } 
    }
}