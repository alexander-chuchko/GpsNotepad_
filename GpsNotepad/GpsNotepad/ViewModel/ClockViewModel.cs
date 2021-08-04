using GpsNotepad.Enum;
using GpsNotepad.Services.Theme;
using GpsNotepad.Services.TimeZone;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class ClockViewModel: BaseViewModel, INavigatedAware
    {
        IThemeService _themeService;

        private string _color;
        public ClockViewModel(INavigationService navigationService, IThemeService themeService) :base(navigationService)
        {
            _themeService = themeService;
        }

        #region---PublicProperties---

        private bool _IsDarkTheme;
        public bool IsDarkTheme
        {
            get { return _IsDarkTheme; }
            set { SetProperty(ref _IsDarkTheme, value); }
        }

        private string _Minutes="32";
        public string Minutes
        {
            get { return _Minutes; }
            set { SetProperty(ref _Minutes, value); }
        }

        private string _Hours="15";
        public string Hours
        {
            get { return _Hours; }
            set { SetProperty(ref _Hours, value); }
        }

        private ICommand _TapBackCommand;

        public ICommand TapBackCommand => _TapBackCommand ?? new Command(OnTapBack);

        #endregion


        #region  ---  Methods   ---
        private async void OnTapBack()
        {
            await _navigationService.GoBackAsync();
        }

        #endregion

        #region--Iterface INavigatedAware implementation--
        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        #endregion
    }
}
