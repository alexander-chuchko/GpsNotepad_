using GpsNotepad.Helpers;
using GpsNotepad.Service;
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
    public class SignUpViewModel : BaseViewModel
    {
        #region ---   PrivateFields   ---

        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IPageDialogService _pageDialogService;

        private string _name;
        private string _emailAddress;
        private string _password;
        private string _confirmPasword;
        private bool _isEnabled;
        private string _placeholderForEmail = ListOfConstants.PlaceholderEnterEmail;
        private string _placeholderForName = ListOfConstants.PlaceholderEnterName;
        private string _imageSourceForEmail = ListOfConstants.ButtonClear;
        private string _imageSourceForName = ListOfConstants.ButtonClear;
        private string _errorName = string.Empty;
        private string _errorEmail = string.Empty;
        private bool _isTapedImageOfName;
        private bool _isTapedImageOfEmail;
        private Color _nameBorderColor = Color.LightGray;
        private Color _emailBorderColor = Color.LightGray;

        #endregion
        public SignUpViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IUserService userService, IPageDialogService pageDialogService) : base(navigationService)
        {
            IsEnabled = false;
            _authenticationService = authenticationService;
            _userService = userService;
            _pageDialogService = pageDialogService;
        }

        #region ---   PublicProperties   ---

        public string PlaceholderForEmail
        {
            get { return _placeholderForEmail; }
            set { SetProperty(ref _placeholderForEmail, value); }
        }
        public string PlaceholderForName
        {
            get { return _placeholderForName; }
            set { SetProperty(ref _placeholderForName, value); }
        }
        
        public string ImageSourceForEmail
        {
            get { return _imageSourceForEmail; }
            set { SetProperty(ref _imageSourceForEmail, value); }
        }
        public string ImageSourceForName
        {
            get { return _imageSourceForName; }
            set { SetProperty(ref _imageSourceForName, value); }
        }
       
        public string ErrorName
        {
            get { return _errorName; }
            set { SetProperty(ref _errorName, value); }
        }
        public string ErrorEmail
        {
            get { return _errorEmail; }
            set { SetProperty(ref _errorEmail, value); }
        }

        public bool IsTapedImageOfName
        {
            get { return _isTapedImageOfName; }
            set { SetProperty(ref _isTapedImageOfName, value); }
        }
        public bool IsTapedImageOfEmail
        {
            get { return _isTapedImageOfEmail; }
            set { SetProperty(ref _isTapedImageOfEmail, value); }
        }

        public Color NameBorderColor
        {
            get { return _nameBorderColor; }
            set { SetProperty(ref _nameBorderColor, value); }
        }
        public Color EmailBorderColor
        {
            get { return _emailBorderColor; }
            set { SetProperty(ref _emailBorderColor, value); }
        }

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

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        private ICommand _NavigationSignInCommand;
        public ICommand NavigationToSingUpCommand => _NavigationSignInCommand ?? (_NavigationSignInCommand = new Command(OnNavigationSignIn));

        private ICommand _CheckDataCommand;
        public ICommand CheckDataCommand => _CheckDataCommand ?? (_CheckDataCommand = new DelegateCommand(OnCheckData, CanOnCheckData).ObservesProperty(() => IsEnabled));

        #endregion

        #region  ---  Methods   ---

        private async void OnNavigationSignIn()
        {
            await _navigationService.GoBackAsync();
        }

        private async void NavigationToSignUp2()
        {
            (string, string) userData = (Name, EmailAddress);
            var parametr = new NavigationParameters();
            parametr.Add(ListOfConstants.UserRegistrationData, userData);
            await _navigationService.NavigateAsync(nameof(SignUpView2), parametr);
        }

        private void OnCheckData()
        {
            var result = true;
            if (!Validation.IsValidatedName(Name) && result)
            {
                result = false;
                ErrorName = ListOfConstants.WrongName;
                NameBorderColor = Color.Red;
            }

            if (!Validation.IsValidatedEmail(EmailAddress) && result)
            {
                result = false;
                ErrorEmail = ListOfConstants.WrongEmail;
                EmailBorderColor = Color.Red;
            }

            if (result)
            {
                NavigationToSignUp2();
            }
        }
        private bool CanOnCheckData()
        {
            return IsEnabled;
        }

        #endregion

        #region ---  Overrides  ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(IsTapedImageOfName))
            {
                Name = string.Empty;
            }
            else if (args.PropertyName == nameof(IsTapedImageOfEmail))
            {
                EmailAddress = string.Empty;
            }

            if (args.PropertyName==nameof(Name)&&NameBorderColor == Color.Red && ErrorName != string.Empty)
            {
                NameBorderColor = Color.FromHex("#858E9E");
                ErrorName = string.Empty;
            }
            else if(args.PropertyName == nameof(EmailAddress)&& EmailBorderColor == Color.Red && ErrorEmail != string.Empty)
            {
                EmailBorderColor = Color.FromHex("#858E9E");
                ErrorEmail = string.Empty;
            }
        }

        #endregion
    }
}