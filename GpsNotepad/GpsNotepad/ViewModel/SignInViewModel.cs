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
    public class SignInViewModel : BaseViewModel, INavigatedAware
    {
        #region---PrivateFields---
        private string _emailAddress;
        private string _password;
        private bool _isEnabled;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPageDialogService _pageDialogService;
        private UserModel _userModel;
        #endregion
        public SignInViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IAuthorizationService authorizationService, IUserService userService, IPageDialogService pageDialogService) : base(navigationService)
        {
            IsEnabled = false;
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _userService = userService;
            _pageDialogService = pageDialogService;
            NavigateToListView = new DelegateCommand(ExecuteNavigationToMainList, CanExecuteNavigateToSignUp).ObservesProperty(() => IsEnabled);
            NavigationToSingUp = new Command(ExecuteNavigateToSignUp);
        }
        #region---PublicProperties---
        public ICommand NavigationToSingUp { get; set; }
        public ICommand NavigateToListView { get; set; }

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
        private async void ExecuteNavigateToSignUp()
        {
            await _navigationService.NavigateAsync(($"{ nameof(SignUpView)}"));
        }
        private bool CanExecuteNavigateToSignUp()
        {
            return IsEnabled;
        }
        private void DeletingCurrentUserSettings() //When logging out, delete all user settings
        {
            _authorizationService.Unauthorize();
        }
        private async void ExecuteNavigationToMainList()
        {
            if (await _authenticationService.SignInAsync(EmailAddress, Password))
            {
                await _navigationService.NavigateAsync($"/{ nameof(NavigationPage)}/{ nameof(MainListTabbedPageView)}");
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync(AppResource.invalid_email_or_password, AppResource.invalid_data_entered, "OK");
                EmailAddress = string.Empty;
                Password = string.Empty;
            }
        }
        #endregion
        #region--Iterface INavigatedAware implementation--
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<UserModel>(ListOfNames.newUser, out UserModel userModel))
            {
                UserModel = parameters.GetValue<UserModel>(ListOfNames.newUser);
                EmailAddress = UserModel.Email;
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }
        #endregion
    }
}
