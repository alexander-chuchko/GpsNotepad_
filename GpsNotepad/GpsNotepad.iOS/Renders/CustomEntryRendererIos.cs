using Foundation;
using GpsNotepad.Controls;
using GpsNotepad.iOS.Renders;

using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using static Unity.Storage.RegistrationSet;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRendererIos))]
namespace GpsNotepad.iOS.Renders
{


    public class CustomEntryRendererIos
    {
        /*
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementPropertyChanged(e);

            if (e.OldElement == null)
            {
                Control.Layer.CornerRadius = 10;
                Control.Layer.BorderWidth = 3f;
                Control.Layer.BorderColor = Color.DeepPink.ToCGColor();

                Control.LeftView = new UIKit.UIView(new CGRect(0, 0, 10, 0));
                Control.LeftViewMode = UIKit.UITextFieldViewMode.Always;
            }
        }
        */
    }

}