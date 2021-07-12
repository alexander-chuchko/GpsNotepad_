using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class SettingsViewModel: BaseViewModel
    {
        public SettingsViewModel(INavigationService navigationService):base(navigationService)
        {

        }
        private ICommand _BackTapCommand;
        public ICommand BackTapCommand => _BackTapCommand ?? (_BackTapCommand = new Command(OnBackTapCommand));

        private async void OnBackTapCommand()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
