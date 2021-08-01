

using GpsNotepad.Service.Settings;

namespace GpsNotepad.Service.Authorization
{
    public class AuthorizationService: IAuthorizationService
    {
        private readonly ISettingsManager _settingsManager;

        public AuthorizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _IsAuthorized = settingsManager.AuthorizedUserID != default(int) ? true : false;
        }

        private bool _IsAuthorized;
        public bool IsAuthorized
        {
            get { return _IsAuthorized; }
        }

        public void Unauthorize()
        {
            _settingsManager.ClearData();
        }
    }
}
