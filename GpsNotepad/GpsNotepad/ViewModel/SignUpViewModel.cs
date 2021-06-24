using GpsNotepad.Helpers;
using GpsNotepad.Model;
using GpsNotepad.Recource;
using GpsNotepad.Service;
using GpsNotepad.Service.User;
using GpsNotepad.View;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class SignUpViewModel: BaseViewModel
    {
        #region---PrivateFields---
        private string _emailAddress;
        private string _password;
        private string _confirmPasword;
        private bool _isEnabled;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IPageDialogService _pageDialogService;
        private UserModel _userModel;
        #endregion
        public SignUpViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IUserService userService, IPageDialogService pageDialogService) : base(navigationService)
        {
            IsEnabled = false;
            EmailAddress = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            _authenticationService = authenticationService;
            _userService = userService;
            _pageDialogService = pageDialogService;
            SignUpCommand = new DelegateCommand(ExecuteNavigationToSignUp, CanExecuteNavigationToSignUp).ObservesProperty(() => IsEnabled);
        }
        #region---PublicProperties---
        public ICommand SignUpCommand { get; set; }
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { SetProperty(ref _emailAddress, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public string ConfirmPassword
        {
            get { return _confirmPasword; }
            set { SetProperty(ref _confirmPasword, value); }
        }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }
        public UserModel UserModel
        {
            set { _userModel = value; }
            get { return _userModel; }
        }
        #endregion
        #region---Methods---
        private async void ExecuteGoBack()
        {
            var parametr = new NavigationParameters();
            parametr.Add(ListOfNames.NewUser, UserModel);
            //await _navigationService.NavigateAsync($"/{ nameof(NavigationPage)}/{ nameof(SignInView)}", parametr);
            await _navigationService.GoBackAsync(parametr);
        }
        private void ClearFields()
        {
            EmailAddress = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }
        private async void ExecuteNavigationToSignUp()
        {
            var result = true;
            if (!Validation.IsValidatedLogin(EmailAddress) && result)
            {
                result = false;
                await _pageDialogService.DisplayAlertAsync(AppResource.requirements_to_email, AppResource.invalid_data_entered, "OK");
            }

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
                UserModel = await _authenticationService.SignUpAsync(EmailAddress, Password);
                if (UserModel != null)
                {
                    var resultOfAction = await _userService.SaveUserModelAsync(UserModel);
                    if (resultOfAction)
                    {
                        ExecuteGoBack();
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(AppResource.this_login_is_already_taken, AppResource.invalid_data_entered, "OK");
                }
            }
            if (!result)
            {
                ClearFields();
            }
        }
        private bool CanExecuteNavigationToSignUp()
        {
            return IsEnabled;
        }
        #endregion

    }
}
