using GpsNotepad.Enum;
using GpsNotepad.Services.Theme;
using GpsNotepad.View;
using Prism.Navigation;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class SettingsViewModel: BaseViewModel, INavigatedAware
    {
        #region---PrivateFields---

        private readonly IThemeService _themeService;

        #endregion

        public SettingsViewModel(INavigationService navigationService, IThemeService themeService) :base(navigationService)
        {
            _themeService = themeService;
            ImageSource = "ic_eye_off.png";
            DisplaySavedPageSettings();
        }

        #region  ---   PublicProperties   ---

        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        private bool _IsCheckedTheme;
        public bool IsCheckedTheme
        {
            get { return _IsCheckedTheme; }
            set { SetProperty(ref _IsCheckedTheme, value); }
        }

        private ICommand _NavigationToColorClockCommand;

        public ICommand NavigationToColorClockCommand => _NavigationToColorClockCommand ?? new Command(OnNavigationToColorClock);


        private ICommand _BackTapCommand;
        public ICommand BackTapCommand => _BackTapCommand ?? new Command(OnBackTapCommand);

        #endregion


        #region   ---   Methods   ---

        private void OnNavigationToColorClock()
        {
            _navigationService.NavigateAsync(nameof(ColorClockView));
        }
        private void DisplaySavedPageSettings()
        {

            EnumSet.Theme themeType = _themeService.GetValueTheme();
            switch (themeType)
            {
                case EnumSet.Theme.Light:
                    IsCheckedTheme = false;
                    break;
                case EnumSet.Theme.Dark:
                    IsCheckedTheme = true;
                    break;
            }
        }
        private void ChangeTheme()
        {
            if(IsCheckedTheme)
            {
                _themeService.PerformThemeChange(EnumSet.Theme.Dark);
            }
            else
            {
                _themeService.PerformThemeChange(EnumSet.Theme.Light);
            }
        }
        private async void OnBackTapCommand()
        {
            await _navigationService.GoBackAsync();
        }

        private void SaveThemeSettings()
        {
            if (IsCheckedTheme)
            {
                if (_themeService.GetValueTheme() != EnumSet.Theme.Dark)
                {
                    _themeService.SetValueTheme(EnumSet.Theme.Dark);
                }
            }
            else if(_themeService.GetValueTheme()!=EnumSet.Theme.Light)
            {
                _themeService.SetValueTheme(EnumSet.Theme.Light);
            }
        }
        #endregion


        #region    ---   Overriding   ---
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(IsCheckedTheme))
            {
                ChangeTheme();
            }
        }

        #endregion


        #region  ---   Iterface INavigatedAware implementation   ---

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            SaveThemeSettings();
        }

        #endregion
    }
}
