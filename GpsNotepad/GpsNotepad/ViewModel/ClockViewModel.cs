using GpsNotepad.Enum;
using GpsNotepad.Helpers;
using GpsNotepad.Services.Theme;
using GpsNotepad.Services.TimeZone;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class ClockViewModel: BaseViewModel, INavigatedAware
    {
        #region   ---    PrivateFields   ---

        IThemeService _themeService;
        ITimeZoneService _timeZoneService;

        #endregion

        public ClockViewModel(INavigationService navigationService,
            IThemeService themeService,
            ITimeZoneService timeZoneService) :base(navigationService)
        {
            _timeZoneService = timeZoneService;
            _themeService = themeService;
            OnSettingColors();
        }

        #region   ---  PublicProperties  ---


        private DateTimeOffset _DateTime;
        public DateTimeOffset DateTime
        {
            get { return _DateTime; }
            set { SetProperty(ref _DateTime, value); }
        }


        private bool _IsDarkTheme;
        public bool IsDarkTheme
        {
            get { return _IsDarkTheme; }
            set { SetProperty(ref _IsDarkTheme, value); }
        }


        private string _CurrentTime;
        public string CurrentTime
        {
            get { return _CurrentTime; }
            set { SetProperty(ref _CurrentTime, value); }
        }


        private string _TypeTime;
        public string TypeTime
        {
            get { return _TypeTime; }
            set { SetProperty(ref _TypeTime, value); }
        }


        private string _ClockTextColor;
        public string ClockTextColor
        {
            get { return _ClockTextColor; }
            set { SetProperty(ref _ClockTextColor, value); }
        }


        private string _ClockColorBackground;
        public string ClockColorBackground
        {
            get { return _ClockColorBackground; }
            set { SetProperty(ref _ClockColorBackground, value); }
        }


        private string _MinuteColor;
        public string MinuteColor
        {
            get { return _MinuteColor; }
            set { SetProperty(ref _MinuteColor, value); }
        }


        private string _HourColor;
        public string HourColor
        {
            get { return _HourColor; }
            set { SetProperty(ref _HourColor, value); }
        }


        private string _SecondsAndOutlineColor;
        public string SecondsAndOutlineColor
        {
            get { return _SecondsAndOutlineColor; }
            set { SetProperty(ref _SecondsAndOutlineColor, value); }
        }


        private string _DayLightName;
        public string DayLightName
        {
            get { return _DayLightName; }
            set { SetProperty(ref _DayLightName, value); }
        }


        private ICommand _TapBackCommand;
        public ICommand TapBackCommand => _TapBackCommand ?? new Command(OnTapBack);

        #endregion


        #region  ---  Methods   ---

        private void UpdateClockTime()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if(DateTimeOffset.Now.Second==0)
                {
                    DateTimeOffset dateTime = DateTimeOffset.Now.AddHours(DateTime.Offset.Hours);
                    CurrentTime = dateTime.ToString("HH:mm");
                }

                return true;
            });
        }

        private void OnSettingColors()
        {
            EnumSet.ClockСolor clockСolor = _themeService.GetValueColorClock();

            switch (clockСolor)
            {

                case EnumSet.ClockСolor.Blue:

                    MinuteColor=Application.Current.Resources["minuteColorBlue"].ToString();
                    SecondsAndOutlineColor= Application.Current.Resources["secondsAndOutlineColorBlue"].ToString();
                    HourColor= Application.Current.Resources["hourColorBlue"].ToString();
                    break;

                case EnumSet.ClockСolor.Red:

                    MinuteColor=Application.Current.Resources["minuteColorRed"].ToString();
                    SecondsAndOutlineColor = Application.Current.Resources["secondsAndOutlineColorRed"].ToString();
                    HourColor = Application.Current.Resources["hourColorRed"].ToString();
                    break;

                case EnumSet.ClockСolor.Green:

                    MinuteColor=Application.Current.Resources["minuteColorGreen"].ToString();
                    SecondsAndOutlineColor = Application.Current.Resources["secondsAndOutlineColorGreen"].ToString();
                    HourColor = Application.Current.Resources["hourColorGreen"].ToString();
                    break;
            }

            EnumSet.Theme theme=_themeService.GetValueTheme();

            switch (theme)
            {

                case EnumSet.Theme.Light:

                    ClockColorBackground = Application.Current.Resources["clockColorBackgroundForThemLight"].ToString();
                    ClockTextColor= Application.Current.Resources["clockTextColorForThemLight"].ToString();
                    break;

                case EnumSet.Theme.Dark:

                    ClockColorBackground = Application.Current.Resources["clockColorBackgroundForThemDark"].ToString();
                    ClockTextColor = Application.Current.Resources["clockTextColorForThemDark"].ToString();
                    break;
            }
        }
        private async void OnTapBack()
        {
            await _navigationService.GoBackAsync();
        }

        #endregion


        #region  ---  Iterface INavigatedAware implementation   ---

        public void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.TryGetValue<(DateTimeOffset, TimeZoneInfo)>(ListOfConstants.TimeZone, out (DateTimeOffset, TimeZoneInfo) clockData))
            {
                DateTime = clockData.Item1;
                DayLightName = clockData.Item2.DaylightName;
                CurrentTime = clockData.Item1.ToString("HH:mm");

                UpdateClockTime();
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        { 
        }

        #endregion
    }
}
