using GpsNotepad.Service.Settings;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Services.Camera
{
    public class CameraService: ICameraService
    {
        private readonly ISettingsManager _settingsManager;
        public CameraService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }
        public void SaveDataCameraPosition(CameraPosition cameraPosition)
        {
            _settingsManager.BearingCameraPosition = cameraPosition.Bearing;
            _settingsManager.LatitudeCameraPosition = cameraPosition.Target.Latitude;
            _settingsManager.LongitudeCameraPosition = cameraPosition.Target.Longitude;
            _settingsManager.TiltCameraPosition = cameraPosition.Tilt;
            _settingsManager.ZoomCameraPosition = cameraPosition.Zoom;
        }
        public CameraPosition GetDataCameraPosition()
        {
            CameraPosition cameraPosition = null;
            
            if (_settingsManager.ZoomCameraPosition != default(double))
            {
                Position position = new Position(_settingsManager.LatitudeCameraPosition, _settingsManager.LongitudeCameraPosition);
                cameraPosition = new CameraPosition
                (
                    position,
                    _settingsManager.ZoomCameraPosition,
                    _settingsManager.BearingCameraPosition,
                    _settingsManager.TiltCameraPosition
                );
            }
            else
            {
                cameraPosition = new CameraPosition(new Position(default(double), default(double)), 2);
            }
            return cameraPosition;
        }
    }
}