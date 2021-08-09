using GpsNotepad.Helpers;
using GpsNotepad.Model.ImagePin;
using GpsNotepad.Model.Pin;
using GpsNotepad.View;
using GpsNotepad.ViewModel;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.Popup
{
    public class PopupViewModel: BaseViewModel, INavigatedAware
    {
        public PopupViewModel(INavigationService navigationService):base(navigationService)
        {
        }

        private bool _IsVisibleCollectionView;

        public bool IsVisibleCollectionView
        {
            get { return _IsVisibleCollectionView; }
            set { SetProperty(ref _IsVisibleCollectionView, value); }
        }

        private ObservableCollection<ImagePinViewModel> _ImagePinViewModels;

        public ObservableCollection<ImagePinViewModel> ImagePinViewModels
        {
            get => _ImagePinViewModels;
            set => SetProperty(ref _ImagePinViewModels, value);
        }

        private double _WidhtDisplay;
        public double WidhtDisplay
        {
            get { return _WidhtDisplay; }
            set { SetProperty(ref _WidhtDisplay, value); }
        }

        private ImagePinViewModel _SelectedImage;
        public ImagePinViewModel SelectedImage
        {
            get => _SelectedImage;
            set => SetProperty(ref _SelectedImage, value);
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
        private ICommand _TapClockButtonCommand;
        public ICommand TapClockButtonCommand => _TapClockButtonCommand ?? new Command(OnTapClockButton);

        private ICommand _NavigationToMainMapCommand;
        public ICommand NavigationToMainMapCommand => _NavigationToMainMapCommand ?? (_NavigationToMainMapCommand = new Command(OnNavigationToMainMap));

        private async void OnTapClockButton()
        {

            await _navigationService.GoBackAsync();

        }
        private void OnNavigationToMainMap()
        {
            _navigationService.GoBackAsync();
        }

        private async void OnImagePreview()
        {
            (ImagePinViewModel, ObservableCollection<ImagePinViewModel>) imagePinData = (SelectedImage, ImagePinViewModels);
            var parameters = new NavigationParameters();

            parameters.Add(ListOfConstants.SelectedImage, imagePinData);
            await _navigationService.NavigateAsync(nameof(PhotoView), parameters, useModalNavigation:true);

        }

        #region ---  Overrides  ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if(args.PropertyName==nameof(SelectedImage))
            {
                OnImagePreview();
            }
        }

        #endregion


        #region--Iterface INavigatedAware implementation--
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<(ObservableCollection<ImagePinViewModel>, PinViewModel)>(ListOfConstants.SelectedPinData, out (ObservableCollection<ImagePinViewModel>, PinViewModel) pinData))
            {
                if(pinData.Item1!=null)
                {
                    ImagePinViewModels = pinData.Item1;
                }

                PinViewModell = pinData.Item2;

                if(PinViewModell != null)
                {
                    LabelSelectedPin = PinViewModell.Label;
                    AddressSelectedPin = PinViewModell.Address;
                    LatitudeSelectedPin = PinViewModell.Latitude;
                    LongitudeSelectedPin = PinViewModell.Longitude;
                    DescriptionSelectedPin = PinViewModell.Description;

                    if (ImagePinViewModels.Count != 0)
                    {
                        IsVisibleCollectionView = true;
                    }
                    else
                    {
                        IsVisibleCollectionView = false;
                    }
                }
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }
        #endregion
    }
}
