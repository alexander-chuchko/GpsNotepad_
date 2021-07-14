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

        private string _password;
        private string _confirmPasword;
        private bool _isEnabled;
        private string _emailAddress;
        private string _name;

        private string _placeholderForPassword = ListOfConstants.PlaceholderEnterPassword;
        private string _placeholderForConfirmPassword = ListOfConstants.PlaceholderEnterConfirmPassword;

        private Color _passwordBorderColor = Color.LightGray;
        private Color _passwordConfirmBorderColor = Color.LightGray;

        private string _errorConfirmPassword = string.Empty;
        private string _errorPassword = string.Empty;

        private string _imageSourceForPassword = ListOfConstants.ButtonEye;
        private string _imageSourceForConfirmPassword = ListOfConstants.ButtonEye;

        private UserModel _userModel;
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
        private bool _isPassword;
        public bool IsPassword
        {
            get { return _isPassword; }
            set { SetProperty(ref _isPassword, value); }
        }

        private bool _isConfirmPassword;
        public bool IsConfirmPassword
        {
            get { return _isConfirmPassword; }
            set { SetProperty(ref _isConfirmPassword, value); }
        }

        private bool _isTapedImageOfConfirmPassword;
        public bool IsTapedImageOfConfirmPassword
        {
            get { return _isTapedImageOfConfirmPassword; }
            set { SetProperty(ref _isTapedImageOfConfirmPassword, value); }
        }

        private bool _isTapedImageOfPassword;
        public bool IsTapedImageOfPassword
        {
            get { return _isTapedImageOfPassword; }
            set { SetProperty(ref _isTapedImageOfPassword, value); }
        }

        public string ImageSourceForPassword
        {
            get { return _imageSourceForPassword; }
            set { SetProperty(ref _imageSourceForPassword, value); }
        }

        public string ImageSourceForConfirmPassword
        {
            get { return _imageSourceForConfirmPassword; }
            set { SetProperty(ref _imageSourceForConfirmPassword, value); }
        }

        public string ErrorPassword
        {
            get { return _errorPassword; }
            set { SetProperty(ref _errorPassword, value); }
        }

        public string ErrorConfirmPassword
        {
            get { return _errorConfirmPassword; }
            set { SetProperty(ref _errorConfirmPassword, value); }
        }

        public Color PasswordBorderColor
        {
            get { return _passwordBorderColor; }
            set { SetProperty(ref _passwordBorderColor, value); }
        }

        public Color ConfirmPasswordBorderColor
        {
            get { return _passwordConfirmBorderColor; }
            set { SetProperty(ref _passwordConfirmBorderColor, value); }
        }
        public string PlaceholderForPassword
        {
            get { return _placeholderForPassword; }
            set { SetProperty(ref _placeholderForPassword, value); }
        }

        public string PlaceholderForConfirmPassword
        {
            get { return _placeholderForConfirmPassword; }
            set { SetProperty(ref _placeholderForConfirmPassword, value); }
        }
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
                ErrorConfirmPassword =ListOfConstants.WrongConfirmPassword;
                ConfirmPasswordBorderColor = Color.Red;
                //await _pageDialogService.DisplayAlertAsync(AppResource.requirements_for_password_and_confirm_password, AppResource.invalid_data_entered, "OK");
            }

            if (result && !Validation.IsValidatedPassword(Password))
            {
                result = false;
                ErrorPassword = ListOfConstants.WrongPassword;
                PasswordBorderColor = Color.Red;
                //await _pageDialogService.DisplayAlertAsync(AppResource.requirements_to_password, AppResource.invalid_data_entered, "OK");
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
                //ClearFields();
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

            if (args.PropertyName == nameof(Name) && PasswordBorderColor == Color.Red && ErrorPassword != string.Empty)
            {
                ConfirmPasswordBorderColor = Color.FromHex("#858E9E");
                ErrorPassword = string.Empty;
            }
            else if (args.PropertyName == nameof(EmailAddress) && ConfirmPasswordBorderColor == Color.Red && ErrorConfirmPassword != string.Empty)
            {
                PasswordBorderColor = Color.FromHex("#858E9E");
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
