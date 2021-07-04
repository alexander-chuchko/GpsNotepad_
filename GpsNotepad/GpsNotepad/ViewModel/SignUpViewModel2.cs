using GpsNotepad.Helpers;
using GpsNotepad.Model;
using GpsNotepad.Recource;
using GpsNotepad.Service;
using GpsNotepad.Service.Authorization;
using GpsNotepad.Service.User;
using GpsNotepad.View;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class SignUpViewModel2: BaseViewModel, INavigatedAware
    {
        #region---PrivateFields---
        private string _password;
        private string _confirmPasword;
        private bool _isEnabled;
        private string _emailAddress;
        private string _name;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPageDialogService _pageDialogService;
        private UserModel _userModel;
        #endregion
        public SignUpViewModel2(INavigationService navigationService, IAuthenticationService authenticationService, IAuthorizationService authorizationService, IUserService userService, IPageDialogService pageDialogService) : base(navigationService)
        {
            IsEnabled = false;
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _userService = userService;
            _pageDialogService = pageDialogService;
            CheckDataCommand = new DelegateCommand(OnCheckData, CanOnCheckData).ObservesProperty(() => IsEnabled);
            NavigateToSignUpCommand = new Command(OnNavigateToSignUp);
        }

        #region---PublicProperties---
        public ICommand NavigationToSingUp { get; set; }
        public ICommand CheckDataCommand { get; set; }
        public ICommand NavigateToSignUpCommand { get; set; }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { SetProperty(ref _emailAddress, value); }
        }

        public string ConfirmPassword
        {
            get { return _confirmPasword; }
            set { SetProperty(ref _confirmPasword, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }
        public UserModel UserModel
        {
            get { return _userModel; }
            set { SetProperty(ref _userModel, value); }
        }
        #endregion
        #region---Methods---
        private async void OnNavigateToSignUp()
        {
            await _navigationService.GoBackAsync();
        }
        private async void NavigationToSignIn()
        {
            var parametr = new NavigationParameters();
            parametr.Add(ListOfNames.NewUser, UserModel);
            //await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInView)}", parametr);
            await _navigationService.NavigateAsync($"/{nameof(SignInView)}", parametr);
        }
      
        private void ClearFields()
        {
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        private async void OnCheckData()
        {
            var result = true;

            if (result && !Validation.CompareStrings(Password, ConfirmPassword))
            {
                result = false;
                await _pageDialogService.DisplayAlertAsync(AppResource.requirements_for_password_and_confirm_password, AppResource.invalid_data_entered, "OK");
            }

            if (result && !Validation.IsValidatedPassword(Password))
            {
                result = false;
                await _pageDialogService.DisplayAlertAsync(AppResource.requirements_to_password, AppResource.invalid_data_entered, "OK");
            }

            if (result)
            {
                UserModel = await _authenticationService.SignUpAsync(EmailAddress, Password, Name);
                if (UserModel != null)
                {
                    var resultOfAction = await _userService.SaveUserModelAsync(UserModel);
                    if (resultOfAction)
                    {
                        NavigationToSignIn();
                    }
                }
                else
                {
                    //Correct
                    await _pageDialogService.DisplayAlertAsync(AppResource.this_login_is_already_taken, AppResource.invalid_data_entered, "OK");
                }
            }
            if (!result)
            {
                ClearFields();
            }
        }
        private bool CanOnCheckData()
        {
            return IsEnabled;
        }
        #endregion
        #region--Iterface INavigatedAware implementation--
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<(string, string)>(ListOfNames.UserRegistrationData, out (string, string) userData))
            {
                //UserModel.Name = userData.Item1;
                //UserModel.Email = userData.Item2;
                Name = userData.Item1;
                EmailAddress = userData.Item2;
            }
        }
        #endregion
    }
}
