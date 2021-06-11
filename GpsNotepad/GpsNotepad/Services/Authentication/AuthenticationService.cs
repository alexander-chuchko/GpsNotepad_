
using GpsNotepad.Model;
using GpsNotepad.Service.Settings;
using GpsNotepad.Service.User;
using System.Threading.Tasks;

namespace GpsNotepad.Service
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly ISettingsManager _settingsManager;
        public AuthenticationService(ISettingsManager settingsManager, IUserService userService)
        {
            _settingsManager = settingsManager;
            _userService = userService;
        }
        public async Task<UserModel> SignUpAsync(string login, string password)
        {
            var uniquenessCheckResult = true;
            UserModel userModel = null;
            var userList = await _userService.GetAllUserModelAsync();
            if (userList != null)
            {
                foreach (var user in userList)
                {
                    if (string.Compare(user.Email, login, true) == 0)
                    {
                        uniquenessCheckResult = false;
                    }
                }
            }
            if(uniquenessCheckResult)
            {
                userModel = new UserModel()
                {
                    Email = login,
                    Password = password
                };
            }
            return userModel;
        }
        public async Task<bool> SignInAsync(string Email, string password)
        {
            var relevanceСheckResult = false;
            var listOfUserModels = await _userService.GetAllUserModelAsync();
            if (listOfUserModels != null)
            {
                foreach (var userModel in listOfUserModels)
                {
                    if (userModel.Email == Email && userModel.Password == password)
                    {
                        _settingsManager.AuthorizedUserID = userModel.Id;
                        relevanceСheckResult = true;
                    }
                }
            }
            return relevanceСheckResult;
        }
    }
}
