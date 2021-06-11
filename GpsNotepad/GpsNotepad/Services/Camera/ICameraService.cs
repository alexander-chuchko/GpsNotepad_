using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Services.Camera
{
    public interface ICameraService
    {
        void SaveDataCameraPosition(CameraPosition cameraPosition);
        CameraPosition GetDataCameraPosition();
    }
}
