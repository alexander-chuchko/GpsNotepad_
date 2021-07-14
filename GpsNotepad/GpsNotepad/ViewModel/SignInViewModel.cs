using GpsNotepad.Helpers;
using GpsNotepad.Model;
using GpsNotepad.Service;
using GpsNotepad.Service.Authorization;
using GpsNotepad.Service.User;
using GpsNotepad.View;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class SignInViewModel : BaseViewModel, INavigatedAware
    {
        #region   ---PrivateFields---

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPageDialogService _pageDialogService;

        private string _emailAddress;
        private string _password;
        private bool _isEnabled;
        private string _placeholderForEmail=ListOfConstants.PlaceholderEnterEmail;
        private string _placeholderForPassword = ListOfConstants.PlaceholderEnterPassword;
        private string _errorEmail =string.Empty;
        private string _errorPassword = string.Empty;
        private string _imageSourceForEmail = ListOfConstants.ButtonClear;
        private string _imageSourceForPassword=ListOfConstants.ButtonEye;
        private Color _emailBorderColor = Color.LightGray;
        private Color _passwordBorderColor = Color.LightGray;
        private UserModel _userModel;

        #endregion

        public SignInViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IAuthorizationService authorizationService, IUserService userService, IPageDialogService pageDialogService) : base(navigationService)
        {
            IsEnabled = false;
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _userService = userService;
            _pageDialogService = pageDialogService;
        }

        #region  ---  PublicProperties  ---

        private bool _isPassword;
        public bool IsPassword
        {
            get { return _isPassword; }
            set { SetProperty(ref _isPassword, value); }
        }

        private bool _isTapedImageOfEmail;
        public bool IsTapedImageOfEmail
        {
            get { return _isTapedImageOfEmail; }
            set { SetProperty(ref _isTapedImageOfEmail, value); }
        }

        private bool _isTapedImageOfPassword;
        public bool IsTapedImageOfPassword
        {
            get { return _isTapedImageOfPassword; }
            set { SetProperty(ref _isTapedImageOfPassword, value); }
        }

        public Color EmailBorderColor
        {
            get { return _emailBorderColor; }
            set { SetProperty(ref _emailBorderColor, value); }
        }

        public Color PasswordBorderColor
        {
            get { return _passwordBorderColor; }
            set { SetProperty(ref _passwordBorderColor, value); }
        }

        public string ImageSourceForEmail
        {
            get { return _imageSourceForEmail; }
            set { SetProperty(ref _imageSourceForEmail, value); }
        }
        public string ImageSourceForPassword
        {
            get { return _imageSourceForPassword; }
            set { SetProperty(ref _imageSourceForPassword, value); }
        }
        public string ErrorEmail
        {
            get { return _errorEmail; }
            set { SetProperty(ref _errorEmail, value); }
        }
        public string ErrorPassword
        {
            get { return _errorPassword; }
            set { SetProperty(ref _errorPassword, value); }
        }
        public string PlaceholderForEmail
        {
            get { return _placeholderForEmail; }
            set { SetProperty(ref _placeholderForEmail, value); }
        }
        public string PlaceholderForPassword
        {
            get { return _placeholderForPassword; }
            set { SetProperty(ref _placeholderForPassword, value); }
        }
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

        private ICommand _NavigationToSingUpCommand;
        public ICommand NavigationToSingUpCommand => _NavigationToSingUpCommand ?? (_NavigationToSingUpCommand = new Command(OnNavigationToSingUp));

        private ICommand _CheckDataCommand;
        public ICommand CheckDataCommand => _CheckDataCommand ?? (_CheckDataCommand = new DelegateCommand(OnCheckData, CanOnCheckData).ObservesProperty(() => IsEnabled));

        private ICommand _NavigateToMainPageCommand;
        public ICommand NavigateToMainPageCommand => _NavigateToMainPageCommand ?? (_NavigateToMainPageCommand = new Command(OnNavigateToMainPage));

        #endregion

        #region---Methods---
        private async void OnNavigateToMainPage()
        {
            await _navigationService.GoBackAsync();
        }
        private async void OnNavigationToSingUp()
        {
            await _navigationService.NavigateAsync($"{ nameof(SignUpView)}");
        }
        private bool CanOnCheckData()
        {
            return IsEnabled;
        }
        private void DeletingCurrentUserSettings() //When logging out, delete all user settings
        {
            _authorizationService.Unauthorize();
        }
        private async void OnCheckData()
        {
            if (await _authenticationService.SignInAsync(EmailAddress, Password))
            {
                await _navigationService.NavigateAsync($"{ nameof(TabbedPage1)}");
            }
            else
            {
                EmailBorderColor = Color.Red;
                PasswordBorderColor = Color.Red;
                ErrorEmail= ListOfConstants.WrongEmail;
                ErrorPassword= ListOfConstants.WrongPassword;
            }
        }

        #endregion

        #region -- Overrides --
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if(args.PropertyName== nameof(IsTapedImageOfEmail))
            {
                EmailAddress = string.Empty;
            }

            else if(args.PropertyName==nameof(IsTapedImageOfPassword))
            {
                ImageSourceForPassword = IsTapedImageOfPassword ? ListOfConstants.ButtonEyeOff : ListOfConstants.ButtonEye;
                IsPassword = IsTapedImageOfPassword;
            }
            
            if(args.PropertyName==nameof(EmailAddress)||args.PropertyName==nameof(Password))
            {
                if(EmailBorderColor==Color.Red&&PasswordBorderColor==Color.Red)
                {
                    EmailBorderColor = Color.FromHex("#858E9E");
                    PasswordBorderColor = Color.FromHex("#858E9E");
                    ErrorEmail = string.Empty;
                    ErrorPassword = string.Empty;
                }
            }
        }

        #endregion

        #region--Iterface INavigatedAware implementation--
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<UserModel>(ListOfConstants.NewUser, out UserModel userModel))
            {
                UserModel = parameters.GetValue<UserModel>(ListOfConstants.NewUser);
                EmailAddress = UserModel.Email;
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        #endregion
    }
}
