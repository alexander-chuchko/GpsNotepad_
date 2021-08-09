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
        #region   ---   PrivateFields   ---

        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPageDialogService _pageDialogService;

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

        private bool _IsPassword;
        public bool IsPassword
        {
            get { return _IsPassword; }
            set { SetProperty(ref _IsPassword, value); }
        }


        private bool _IsTapedImageOfEmail;
        public bool IsTapedImageOfEmail
        {
            get { return _IsTapedImageOfEmail; }
            set { SetProperty(ref _IsTapedImageOfEmail, value); }
        }


        private bool _IsTapedImageOfPassword;
        public bool IsTapedImageOfPassword
        {
            get { return _IsTapedImageOfPassword; }
            set { SetProperty(ref _IsTapedImageOfPassword, value); }
        }


        private Color _EmailBorderColor = Color.LightGray;
        public Color EmailBorderColor
        {
            get { return _EmailBorderColor; }
            set { SetProperty(ref _EmailBorderColor, value); }
        }


        private Color _PasswordBorderColor = Color.LightGray;
        public Color PasswordBorderColor
        {
            get { return _PasswordBorderColor; }
            set { SetProperty(ref _PasswordBorderColor, value); }
        }


        private string _ImageSourceForEmail = ListOfConstants.ButtonClear;
        public string ImageSourceForEmail
        {
            get { return _ImageSourceForEmail; }
            set { SetProperty(ref _ImageSourceForEmail, value); }
        }


        private string _ImageSourceForPassword = ListOfConstants.ButtonEye;
        public string ImageSourceForPassword
        {
            get { return _ImageSourceForPassword; }
            set { SetProperty(ref _ImageSourceForPassword, value); }
        }


        private string _ErrorEmail = string.Empty;
        public string ErrorEmail
        {
            get { return _ErrorEmail; }
            set { SetProperty(ref _ErrorEmail, value); }
        }


        private string _ErrorPassword = string.Empty;
        public string ErrorPassword
        {
            get { return _ErrorPassword; }
            set { SetProperty(ref _ErrorPassword, value); }
        }


        private string _PlaceholderForEmail = ListOfConstants.PlaceholderEnterEmail;
        public string PlaceholderForEmail
        {
            get { return _PlaceholderForEmail; }
            set { SetProperty(ref _PlaceholderForEmail, value); }
        }


        private string _PlaceholderForPassword = ListOfConstants.PlaceholderEnterPassword;
        public string PlaceholderForPassword
        {
            get { return _PlaceholderForPassword; }
            set { SetProperty(ref _PlaceholderForPassword, value); }
        }


        private string _EmailAddress;
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { SetProperty(ref _EmailAddress, value); }
        }


        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }


        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { SetProperty(ref _IsEnabled, value); }
        }


        private UserModel _UserModel;
        public UserModel UserModel
        {
            get { return _UserModel; }
            set { SetProperty(ref _UserModel, value); }
        }


        private ICommand _NavigationToSingUpCommand;
        public ICommand NavigationToSingUpCommand => _NavigationToSingUpCommand ?? (_NavigationToSingUpCommand = new Command(OnNavigationToSingUp));

        private ICommand _CheckDataCommand;
        public ICommand CheckDataCommand => _CheckDataCommand ?? (_CheckDataCommand = new DelegateCommand(OnCheckData, CanOnCheckData).ObservesProperty(() => IsEnabled));

        private ICommand _NavigateToMainPageCommand;
        public ICommand NavigateToMainPageCommand => _NavigateToMainPageCommand ?? (_NavigateToMainPageCommand = new Command(OnNavigateToMainPage));

        #endregion

        #region    ---   Methods   ---
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

        private async void OnCheckData()
        {
            if (await _authenticationService.SignInAsync(EmailAddress, Password))
            {
                await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{ nameof(TabbedPage1)}");
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

        #region    ---   Overrides   ---
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

        #region    ---   Iterface INavigatedAware implementation   ---

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
