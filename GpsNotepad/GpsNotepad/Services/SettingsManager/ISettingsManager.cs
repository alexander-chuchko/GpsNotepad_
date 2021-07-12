namespace GpsNotepad.Service.Settings
{
    public interface ISettingsManager
    {
        int AuthorizedUserID { get; set; }
        double BearingCameraPosition { get; set; }
        double LatitudeCameraPosition { get; set; }
        double LongitudeCameraPosition { get; set; }
        double TiltCameraPosition { get; set; }
        double ZoomCameraPosition { get; set; }
        bool IsEnabledUserLocationButton { get; set; }
        string StateOfTextInSearchBar{get; set;}
        int ThemType { get; set; }

        //int SortingType { get; set; }
        //string SelectedLanguage { get; set; }
        void ClearData();
    }
}
