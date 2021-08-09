using GpsNotepad.Service.Settings;

namespace GpsNotepad.Service.Authorization
{
    public class AuthorizationService: IAuthorizationService
    {
        #region   ---   PrivateFields   ---

        private readonly ISettingsManager _settingsManager;

        #endregion

        public AuthorizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _IsAuthorized = settingsManager.AuthorizedUserID != default(int) ? true : false;
        }

        #region   ---   PublicProperties   ---

        private bool _IsAuthorized;
        public bool IsAuthorized
        {
            get { return _IsAuthorized; }
        }

        #endregion

        #region    ---   Methods   ---

        public void Unauthorize()
        {
            _settingsManager.ClearData();
        }

        #endregion
    }
}
