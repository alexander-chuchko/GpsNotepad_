using GpsNotepad.Helpers;
using GpsNotepad.Model;
using GpsNotepad.Model.Pin;
using GpsNotepad.ViewModel;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.Popup
{
    public class PopupViewModel: BaseViewModel, INavigatedAware
    {
        public PopupViewModel(INavigationService navigationService):base(navigationService)
        {

        }

        private string _DescriptionSelectedPin;
        public string DescriptionSelectedPin
        {
            get => _DescriptionSelectedPin;
            set => SetProperty(ref _DescriptionSelectedPin, value);
        }

        private double _LongitudeSelectedPin;
        public double LongitudeSelectedPin
        {
            get { return _LongitudeSelectedPin; }
            set { SetProperty(ref _LongitudeSelectedPin, value); }
        }

        private double _LatitudeSelectedPin;
        public double LatitudeSelectedPin
        {
            get { return _LatitudeSelectedPin; }
            set { SetProperty(ref _LatitudeSelectedPin, value); }
        }
        private string _AddressSelectedPin;
        public string AddressSelectedPin
        {
            get { return _AddressSelectedPin; }
            set { SetProperty(ref _AddressSelectedPin, value); }
        }

        private string _LabelSelectedPin;
        public string LabelSelectedPin
        {
            get { return _LabelSelectedPin; }
            set { SetProperty(ref _LabelSelectedPin, value); }
        }

        private PinViewModel _PinViewModel;
        public PinViewModel PinViewModell
        {
            get { return _PinViewModel; }
            set { SetProperty(ref _PinViewModel, value); }
        }
        private ICommand _NavigationToMainMapCommand;
        public ICommand NavigationToMainMapCommand => _NavigationToMainMapCommand ?? (_NavigationToMainMapCommand = new Command(OnNavigationToMainMap));

        private void OnNavigationToMainMap()
        {
            _navigationService.GoBackAsync();
        }

        #region--Iterface INavigatedAware implementation--
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinViewModel>(ListOfConstants.SelectedPin, out PinViewModel pinViewModel))
            {
                PinViewModell = parameters.GetValue<PinViewModel>(ListOfConstants.SelectedPin);
                if (PinViewModell != null)
                {
                    LabelSelectedPin = PinViewModell.Label;
                    AddressSelectedPin = PinViewModell.Address;
                    LatitudeSelectedPin = PinViewModell.Latitude;
                    LongitudeSelectedPin = PinViewModell.Longitude;
                    DescriptionSelectedPin = PinViewModell.Description;
                }
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }
        #endregion
    }
}
