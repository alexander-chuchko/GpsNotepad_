using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace GpsNotepad.View
{
    public class BaseContentPage:ContentPage
    {
        public BaseContentPage()
        {
            Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(On<iOS>(), true);
        }    
    }
}
