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

        #endregion
        public SignUpViewModel(INavigationService navigationService,
            IAuthenticationService authenticationService,
            IUserService userService, IPageDialogService pageDialogService) : base(navigationService)
        {
            IsEnabled = false;
            _authenticationService = authenticationService;
            _userService = userService;
            _pageDialogService = pageDialogService;
        }

        #region ---   PublicProperties   ---

        private string _PlaceholderForEmail = ListOfConstants.PlaceholderEnterEmail;
        public string PlaceholderForEmail
        {
            get { return _PlaceholderForEmail; }
            set { SetProperty(ref _PlaceholderForEmail, value); }
        }


        private string _PlaceholderForName = ListOfConstants.PlaceholderEnterName;
        public string PlaceholderForName
        {
            get { return _PlaceholderForName; }
            set { SetProperty(ref _PlaceholderForName, value); }
        }


        private string _ImageSourceForEmail = ListOfConstants.ButtonClear;
        public string ImageSourceForEmail
        {
            get { return _ImageSourceForEmail; }
            set { SetProperty(ref _ImageSourceForEmail, value); }
        }


        private string _ImageSourceForName = ListOfConstants.ButtonClear;
        public string ImageSourceForName
        {
            get { return _ImageSourceForName; }
            set { SetProperty(ref _ImageSourceForName, value); }
        }


        private string _ErrorName = string.Empty;
        public string ErrorName
        {
            get { return _ErrorName; }
            set { SetProperty(ref _ErrorName, value); }
        }


        private string _ErrorEmail = string.Empty;
        public string ErrorEmail
        {
            get { return _ErrorEmail; }
            set { SetProperty(ref _ErrorEmail, value); }
        }


        private bool _IsTapedImageOfName;
        public bool IsTapedImageOfName
        {
            get { return _IsTapedImageOfName; }
            set { SetProperty(ref _IsTapedImageOfName, value); }
        }


        private bool _IsTapedImageOfEmail;
        public bool IsTapedImageOfEmail
        {
            get { return _IsTapedImageOfEmail; }
            set { SetProperty(ref _IsTapedImageOfEmail, value); }
        }


        private Color _NameBorderColor = Color.LightGray;
        public Color NameBorderColor
        {
            get { return _NameBorderColor; }
            set { SetProperty(ref _NameBorderColor, value); }
        }


        private Color _EmailBorderColor = Color.LightGray;
        public Color EmailBorderColor
        {
            get { return _EmailBorderColor; }
            set { SetProperty(ref _EmailBorderColor, value); }
        }


        private string _EmailAddress;
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { SetProperty(ref _EmailAddress, value); }
        }


        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }


        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { SetProperty(ref _IsEnabled, value); }
        }

        private ICommand _BackTapCommand;
        public ICommand BackTapCommand => _BackTapCommand ?? (_BackTapCommand = new Command(OnBackTapCommand));

        private ICommand _CheckDataCommand;
        public ICommand CheckDataCommand => _CheckDataCommand ?? (_CheckDataCommand = new DelegateCommand(OnCheckData, CanOnCheckData).ObservesProperty(() => IsEnabled));

        #endregion

        #region  ---  Methods   ---

        private async void OnBackTapCommand()
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
                NameBorderColor = Color.FromHex("#D7DDE8");
                ErrorName = string.Empty;
            }

            else if(args.PropertyName == nameof(EmailAddress)&& EmailBorderColor == Color.Red && ErrorEmail != string.Empty)
            {
                EmailBorderColor = Color.FromHex("#D7DDE8");
                ErrorEmail = string.Empty;
            }
        }

        #endregion
    }
}