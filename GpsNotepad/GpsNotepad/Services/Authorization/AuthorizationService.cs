

using GpsNotepad.Service.Settings;

namespace GpsNotepad.Service.Authorization
{
    public class AuthorizationService: IAuthorizationService
    {
        private readonly ISettingsManager _settingsManager;
        private bool _isAuthorized;
        public AuthorizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _isAuthorized = settingsManager.AuthorizedUserID != 0 ? true : false;
        }

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
        }

        public void Unauthorize()
        {
            _settingsManager.ClearData();
        }
    }
}
