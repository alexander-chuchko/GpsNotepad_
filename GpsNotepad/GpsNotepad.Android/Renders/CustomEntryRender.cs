using Android.Content;
using GpsNotepad.Droid.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRender))]
namespace GpsNotepad.Droid.Renders
{
    public class CustomEntryRender: EntryRenderer
    {
        public CustomEntryRender(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
    }
}