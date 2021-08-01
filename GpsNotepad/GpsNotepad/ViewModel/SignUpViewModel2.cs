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
    public class SignUpViewModel2: BaseViewModel, INavigatedAware
    {
        #region---PrivateFields---

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPageDialogService _pageDialogService;

        #endregion

        public SignUpViewModel2(INavigationService navigationService, IAuthenticationService authenticationService, IAuthorizationService authorizationService, IUserService userService, IPageDialogService pageDialogService) : base(navigationService)
        {
            IsEnabled = false;
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _userService = userService;
            _pageDialogService = pageDialogService;
        }

        #region---PublicProperties---

        private bool _IsPassword;
        public bool IsPassword
        {
            get { return _IsPassword; }
            set { SetProperty(ref _IsPassword, value); }
        }

        private bool _IsConfirmPassword;
        public bool IsConfirmPassword
        {
            get { return _IsConfirmPassword; }
            set { SetProperty(ref _IsConfirmPassword, value); }
        }

        private bool _IsTapedImageOfConfirmPassword;
        public bool IsTapedImageOfConfirmPassword
        {
            get { return _IsTapedImageOfConfirmPassword; }
            set { SetProperty(ref _IsTapedImageOfConfirmPassword, value); }
        }

        private bool _IsTapedImageOfPassword;
        public bool IsTapedImageOfPassword
        {
            get { return _IsTapedImageOfPassword; }
            set { SetProperty(ref _IsTapedImageOfPassword, value); }
        }

        private string _ImageSourceForPassword = ListOfConstants.ButtonEye;
        public string ImageSourceForPassword
        {
            get { return _ImageSourceForPassword; }
            set { SetProperty(ref _ImageSourceForPassword, value); }
        }

        private string _ImageSourceForConfirmPassword = ListOfConstants.ButtonEye;
        public string ImageSourceForConfirmPassword
        {
            get { return _ImageSourceForConfirmPassword; }
            set { SetProperty(ref _ImageSourceForConfirmPassword, value); }
        }

        private string _ErrorPassword = string.Empty;
        public string ErrorPassword
        {
            get { return _ErrorPassword; }
            set { SetProperty(ref _ErrorPassword, value); }
        }

        private string _ErrorConfirmPassword = string.Empty;
        public string ErrorConfirmPassword
        {
            get { return _ErrorConfirmPassword; }
            set { SetProperty(ref _ErrorConfirmPassword, value); }
        }

        private Color _PasswordBorderColor = Color.LightGray;
        public Color PasswordBorderColor
        {
            get { return _PasswordBorderColor; }
            set { SetProperty(ref _PasswordBorderColor, value); }
        }

        private Color _PasswordConfirmBorderColor = Color.LightGray;
        public Color ConfirmPasswordBorderColor
        {
            get { return _PasswordConfirmBorderColor; }
            set { SetProperty(ref _PasswordConfirmBorderColor, value); }
        }

        private string _PlaceholderForPassword = ListOfConstants.PlaceholderEnterPassword;
        public string PlaceholderForPassword
        {
            get { return _PlaceholderForPassword; }
            set { SetProperty(ref _PlaceholderForPassword, value); }
        }

        private string _PlaceholderForConfirmPassword = ListOfConstants.PlaceholderEnterConfirmPassword;
        public string PlaceholderForConfirmPassword
        {
            get { return _PlaceholderForConfirmPassword; }
            set { SetProperty(ref _PlaceholderForConfirmPassword, value); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private string _EmailAddress;
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { SetProperty(ref _EmailAddress, value); }
        }

        private string _ConfirmPasword;
        public string ConfirmPassword
        {
            get { return _ConfirmPasword; }
            set { SetProperty(ref _ConfirmPasword, value); }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }

        private UserModel _UserModel;
        public UserModel UserModel
        {
            get { return _UserModel; }
            set { SetProperty(ref _UserModel, value); }
        }

        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { SetProperty(ref _IsEnabled, value); }
        }

        private ICommand _NavigateToSignUpCommand;
        public ICommand NavigateToSignUpCommand => _NavigateToSignUpCommand ?? (_NavigateToSignUpCommand = new Command(OnNavigateToSignUp));

        private ICommand _CheckDataCommand;
        public ICommand CheckDataCommand => _CheckDataCommand ?? (_CheckDataCommand = new DelegateCommand(OnCheckData, CanOnCheckData).ObservesProperty(() => IsEnabled));

        #endregion

        #region---Methods---

        private async void OnNavigateToSignUp()
        {
            await _navigationService.GoBackAsync();
        }
        private async void NavigationToSignIn()
        {
            var parametr = new NavigationParameters();
            parametr.Add(ListOfConstants.NewUser, UserModel);
            await _navigationService.NavigateAsync($"/{nameof(SignInView)}", parametr);
        }

        private async void OnCheckData()
        {
            var result = true;

            if (result && !Validation.CompareStrings(Password, ConfirmPassword))
            {
                result = false;
                ErrorConfirmPassword =ListOfConstants.WrongConfirmPassword;
                ConfirmPasswordBorderColor = Color.Red;
            }

            if (result && !Validation.IsValidatedPassword(Password))
            {
                result = false;
                ErrorPassword = ListOfConstants.WrongPassword;
                PasswordBorderColor = Color.Red;
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
            }
        }
        private bool CanOnCheckData()
        {
            return IsEnabled;
        }

        #endregion

        #region -- Overrides --
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsTapedImageOfPassword))
            {
                ImageSourceForPassword = IsTapedImageOfPassword ? ListOfConstants.ButtonEyeOff : ListOfConstants.ButtonEye;
                IsPassword = IsTapedImageOfPassword;
            }
            else if(args.PropertyName == nameof(IsTapedImageOfConfirmPassword))
            {
                ImageSourceForConfirmPassword = IsTapedImageOfConfirmPassword ? ListOfConstants.ButtonEyeOff : ListOfConstants.ButtonEye;
                IsConfirmPassword = IsTapedImageOfConfirmPassword;
            }

            if (args.PropertyName == nameof(Password) && PasswordBorderColor == Color.Red && ErrorPassword != string.Empty)
            {
                PasswordBorderColor = Color.FromHex("#D7DDE8");
                ErrorPassword = string.Empty;
            }
            else if (args.PropertyName == nameof(ConfirmPassword) && ConfirmPasswordBorderColor == Color.Red && ErrorConfirmPassword != string.Empty)
            {
                ConfirmPasswordBorderColor = Color.FromHex("#D7DDE8");
                ErrorConfirmPassword = string.Empty;
            }
        }

        #endregion

        #region---   Iterface INavigatedAware implementation   ---

        public void OnNavigatedFrom(INavigationParameters parameters)
        { 
        }
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<(string, string)>(ListOfConstants.UserRegistrationData, out (string, string) userData))
            {
                Name = userData.Item1;
                EmailAddress = userData.Item2;
            }
        }

        #endregion
    }
}
