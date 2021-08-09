using GpsNotepad.Enum;
using GpsNotepad.Services.Theme;
using Prism.Navigation;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class ColorClockViewModel: BaseViewModel
    {
        #region ---   PrivateFields   ---

        private readonly IThemeService _themeService;

        #endregion

        public ColorClockViewModel(INavigationService navigationService, IThemeService themeService):base(navigationService)
        {
            _themeService = themeService;
            DisplaySavedPageSettings();
        }

        #region  ---   PublicProperties   ---

        private bool _IsCheckedGreen;
        public bool IsCheckedGreen
        {
            get { return _IsCheckedGreen; }
            set { SetProperty(ref _IsCheckedGreen, value); }
        }


        private bool _IsCheckedRed;
        public bool IsCheckedRed
        {
            get { return _IsCheckedRed; }
            set { SetProperty(ref _IsCheckedRed, value); }
        }


        private bool _IsCheckedBlue;
        public bool IsCheckedBlue
        {
            get { return _IsCheckedBlue; }
            set { SetProperty(ref _IsCheckedBlue, value); }
        }


        private ICommand _BackTapCommand;
        public ICommand BackTapCommand => _BackTapCommand ?? new Command(OnBackTapCommand);

        #endregion


        #region   ---   Methods   ---

        private void DisplaySavedPageSettings()
        {
            EnumSet.ClockСolor clockСolor = _themeService.GetValueColorClock();

            switch (clockСolor)
            {

                case EnumSet.ClockСolor.Blue:
                    IsCheckedBlue = true;
                    IsCheckedGreen = false;
                    IsCheckedRed = false;
                    break;

                case EnumSet.ClockСolor.Red:
                    IsCheckedRed = true;
                    IsCheckedBlue = false;
                    IsCheckedGreen = false;
                    break;

                case EnumSet.ClockСolor.Green:
                    IsCheckedGreen = true;
                    IsCheckedBlue = false;
                    IsCheckedRed = false;
                    break;
            }
        }

        private async void OnBackTapCommand()
        {
            await _navigationService.GoBackAsync();
        }

        #endregion


        #region    ---   Overriding   ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(IsCheckedBlue)&&IsCheckedBlue)
            {
                _themeService.SetValueColorClock(EnumSet.ClockСolor.Blue);

                IsCheckedGreen = false;
                IsCheckedRed = false;
            }
            else if(args.PropertyName == nameof(IsCheckedRed) && IsCheckedRed)
            {
                _themeService.SetValueColorClock(EnumSet.ClockСolor.Red);

                IsCheckedBlue = false;
                IsCheckedGreen = false;
            }
            else if (args.PropertyName == nameof(IsCheckedGreen) && IsCheckedGreen)
            {
                _themeService.SetValueColorClock(EnumSet.ClockСolor.Green);

                IsCheckedBlue = false;
                IsCheckedRed = false;
            }
        }

        #endregion

    }
}
