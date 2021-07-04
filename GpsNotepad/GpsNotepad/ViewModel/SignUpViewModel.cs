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
        private string _name;
        private string _emailAddress;
        private string _password;
        private string _confirmPasword;
        private bool _isEnabled;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IPageDialogService _pageDialogService;
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
            CheckDataCommand = new DelegateCommand(OnCheckData, CanOnCheckData).ObservesProperty(() => IsEnabled);
            NavigationSignInCommand = new Command(OnNavigationSignIn);

        }


        #region---PublicProperties---
        public ICommand CheckDataCommand { get; set; }
        public ICommand NavigationSignInCommand { get; set; }
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { SetProperty(ref _emailAddress, value); }
        }
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
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
        #endregion
        #region---Methods---
        private async void OnNavigationSignIn()
        {
            await _navigationService.GoBackAsync();
        }
        private async void NavigationToSignUp2()
        {
            (string, string) userData = (Name, EmailAddress);
            var parametr = new NavigationParameters();
            parametr.Add(ListOfNames.UserRegistrationData, userData);
            await _navigationService.NavigateAsync(nameof(SignUpView2), parametr);
        }
        private void ClearFields()
        {
            //To Do
            EmailAddress = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }
        private async void OnCheckData()
        {
            var result = true;
            if(!Validation.IsValidatedName(Name)&&result)
            {
                result = false;
                await _pageDialogService.DisplayAlertAsync(AppResource.requirements_to_email, AppResource.invalid_data_entered, "OK");
            }

            if (!Validation.IsValidatedEmail(EmailAddress) && result)
            {
                result = false;
                await _pageDialogService.DisplayAlertAsync(AppResource.requirements_to_email, AppResource.invalid_data_entered, "OK");
            }

            if (result)
            {
                NavigationToSignUp2();
            }

            if (!result)//Delete
            {
                ClearFields();
            }
        }
        private bool CanOnCheckData()
        {
            return IsEnabled;
        }
        #endregion
    }
}
