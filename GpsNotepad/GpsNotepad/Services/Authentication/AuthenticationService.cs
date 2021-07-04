
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
        public async Task<UserModel> SignUpAsync(string email, string password, string name)
        {
            var uniquenessCheckResult = true;
            UserModel userModel = null;
            var userList = await _userService.GetAllUserModelAsync();
            if (userList != null)
            {
                foreach (var user in userList)
                {
                    if (string.Compare(user.Email, email, true) == 0)
                    {
                        uniquenessCheckResult = false;
                    }
                }
            }
            if(uniquenessCheckResult)
            {
                userModel = new UserModel()
                {
                    Email = email,
                    Password = password,
                    Name = name
                };
            }
            return userModel;
        }
        public async Task<bool> SignInAsync(string email, string password)
        {
            var relevanceСheckResult = false;
            var listOfUserModels = await _userService.GetAllUserModelAsync();
            if (listOfUserModels != null)
            {
                foreach (var userModel in listOfUserModels)
                {
                    if (userModel.Email == email && userModel.Password == password)
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
