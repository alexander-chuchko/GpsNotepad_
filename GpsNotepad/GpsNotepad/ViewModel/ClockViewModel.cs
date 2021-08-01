using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class ClockViewModel: BaseViewModel
    {
        public ClockViewModel(INavigationService navigationService):base(navigationService)
        {

        }

        #region---PublicProperties---

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
    }
}
