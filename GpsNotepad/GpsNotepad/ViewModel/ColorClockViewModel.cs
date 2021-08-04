using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class ColorClockViewModel: BaseViewModel
    {
        public ColorClockViewModel(INavigationService navigationService):base(navigationService)
        {

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
        private async void OnBackTapCommand()
        {
            await _navigationService.GoBackAsync();
        }

        #endregion

    }
}
