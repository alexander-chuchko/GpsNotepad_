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
        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public SettingsViewModel(INavigationService navigationService):base(navigationService)
        {
            ImageSource = "ic_eye_off.png";
        }
        private ICommand _BackTapCommand;
        public ICommand BackTapCommand => _BackTapCommand ?? (_BackTapCommand = new Command(OnBackTapCommand));

        private async void OnBackTapCommand()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
