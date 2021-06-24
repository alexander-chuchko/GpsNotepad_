using GpsNotepad.Helpers;
using Xamarin.Essentials;

namespace GpsNotepad.Service.Settings
{
    class SettingsManager : ISettingsManager
    {
        public int AuthorizedUserID 
        {
            get => Preferences.Get(nameof(AuthorizedUserID), default(int));
            set => Preferences.Set(nameof(AuthorizedUserID), value);
        }
        /*Properties of CameraPosition*/
        public double BearingCameraPosition
        {
            get => Preferences.Get(nameof(BearingCameraPosition), default(double));
            set => Preferences.Set(nameof(BearingCameraPosition), value);
        }
        public double LatitudeCameraPosition
        {
            get => Preferences.Get(nameof(LatitudeCameraPosition), default(double));
            set => Preferences.Set(nameof(LatitudeCameraPosition), value);
        }
        public double LongitudeCameraPosition
        {
            get => Preferences.Get(nameof(LongitudeCameraPosition), default(double));
            set => Preferences.Set(nameof(LongitudeCameraPosition), value);
        }
        public double TiltCameraPosition
        {
            get => Preferences.Get(nameof(TiltCameraPosition), default(double));
            set => Preferences.Set(nameof(TiltCameraPosition), value);
        }
        public double ZoomCameraPosition
        {
            get => Preferences.Get(nameof(ZoomCameraPosition), default(double));
            set => Preferences.Set(nameof(ZoomCameraPosition), value);
        }
        public bool IsEnabledUserLocationButton
        {
            get => Preferences.Get(nameof(IsEnabledUserLocationButton), default(bool));
            set => Preferences.Set(nameof(IsEnabledUserLocationButton), value);
        }
        public string StateOfTextInSearchBar
        {
            get => Preferences.Get(nameof(StateOfTextInSearchBar), default(string));
            set => Preferences.Set(nameof(StateOfTextInSearchBar), value);
        }

        /*
        public int SortingType
        {
            get => Preferences.Get(nameof(SortingType), 0);
            set => Preferences.Set(nameof(SortingType), value);
        }

        public string SelectedLanguage
        {
            get =>Preferences.Get(nameof(SelectedLanguage), ListOfNames.english);
            set => Preferences.Set(nameof(SelectedLanguage), value);
        }

        public int ThemType
        {
            get => Preferences.Get(nameof(ThemType), 0);
            set => Preferences.Set(nameof(ThemType), value);
        }
        */
        public void ClearData()
        {
            Preferences.Clear();
        }
    }
}
