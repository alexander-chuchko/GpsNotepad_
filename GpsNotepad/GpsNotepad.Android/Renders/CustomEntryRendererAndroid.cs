using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using GpsNotepad.Controls;
using GpsNotepad.Droid.Renders;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRendererAndroid))]
namespace GpsNotepad.Droid.Renders
{
    class CustomEntryRendererAndroid: EntryRenderer
    {
        public CustomEntryRendererAndroid(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
       {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                //Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
                var gradiendDrawable = new GradientDrawable();
                gradiendDrawable.SetCornerRadius(4);
                gradiendDrawable.SetStroke(5, Android.Graphics.Color.LightGray);
                Control.SetBackground(gradiendDrawable);
                Control.SetPadding(60, Control.PaddingTop, Control.PaddingRight, Control.PaddingBottom);
            }
        }
    }
}