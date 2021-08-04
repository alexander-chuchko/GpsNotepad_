using GpsNotepad.Model.Pin;
using GpsNotepad.Services.Camera;
using GpsNotepad.Services.Pin;
using Prism.Navigation;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.Linq;
using GpsNotepad.Extension;
using GpsNotepad.Model;
using GpsNotepad.Helpers;
using GpsNotepad.View;
using GpsNotepad.Services.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GpsNotepad.Service.Authorization;
using GpsNotepad.Services.ImagesOfPin;
using GpsNotepad.Model.ImagePin;
using GpsNotepad.Services.TimeZone;

namespace GpsNotepad.ViewModel
{
    public class MainMapTabbedPageViewModel: BaseViewModel, INavigatedAware, IInitialize
    {
        #region---PrivateFields---

        private readonly IPinServices _pinServices;
        private readonly ICameraService _cameraService;
        private readonly IPermissionService _permissionService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IImagesPinService _imagesPinService;
        private readonly ITimeZoneService _timeZoneService;

        #endregion

        public MainMapTabbedPageViewModel(INavigationService navigationService,
            ICameraService cameraService,
            IPinServices pinServices,
            IPermissionService permissionService,
            IAuthorizationService authorizationService,
            IImagesPinService imagesPinService,
            ITimeZoneService timeZoneService) : base(navigationService)
        {
            _authorizationService = authorizationService;
            _cameraService = cameraService;
            _pinServices= pinServices;
            _permissionService = permissionService;
            _imagesPinService = imagesPinService;
            _timeZoneService = timeZoneService;
            GetAllPins();
            InitialCameraUpdate = CameraUpdateFactory.NewPosition(new Position(0, 0));
            Task.Run(() => RequestLocationPermission());
            IsVisibleListView = false;
            IsInfoVisible = false;
        }

        #region---PublicProperties---

        private bool _IsVisibleCollectionView;

        public bool IsVisibleCollectionView
        {
            get { return _IsVisibleCollectionView; }
            set { SetProperty(ref _IsVisibleCollectionView, value); }
        }

        private ImagePinViewModel _SelectedImage;
        public ImagePinViewModel SelectedImage
        {
            get => _SelectedImage;
            set => SetProperty(ref _SelectedImage, value);
        }

        private ObservableCollection<ImagePinViewModel> _ImagePinViewModels1;

        public ObservableCollection<ImagePinViewModel> ImagePinViewModels1 
        {
            get => _ImagePinViewModels1;
            set => SetProperty(ref _ImagePinViewModels1, value);
        }

        private int _SizeHightListView;
        public int SizeHightListView
        {
            get => _SizeHightListView;
            set => SetProperty(ref _SizeHightListView, value);
        }
        private int _SizeRow = ListOfConstants.SizeRow;
        public int SizeRow
        {
            get => _SizeRow;
            set => SetProperty(ref _SizeRow, value);
        }

        private bool exitSearch;
        public bool ExitSearch
        {
            get => exitSearch;
            set => SetProperty(ref exitSearch, value);
        }
        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        private bool _IsVisibleScrollView;
        public bool IsVisibleScrollView
        {
            get => _IsVisibleScrollView;
            set => SetProperty(ref _IsVisibleScrollView, value);
        }

        private bool _IsVisibleListView;
        public bool IsVisibleListView
        {
            get => _IsVisibleListView;
            set => SetProperty(ref _IsVisibleListView, value);
        }

        private CameraUpdate _InitialCameraUpdate;
        public CameraUpdate InitialCameraUpdate
        {
            get => _InitialCameraUpdate;
            set => SetProperty(ref _InitialCameraUpdate, value);
        }

        private Position _MovingCameraPosition;
        public Position MovingCameraPosition
        {
            get => _MovingCameraPosition;
            set => SetProperty(ref _MovingCameraPosition, value);
        }

        private string _PathImageOfLocation = "icons_mylocation_off.png";
        public string PathImageOfLocation
        {
            get { return _PathImageOfLocation; }
            set { SetProperty(ref _PathImageOfLocation, value); }
        }

        private List<Pin> _PinViewModelList;
        public List<Pin> PinViewModelList
        {
            get { return _PinViewModelList; }
            set { SetProperty(ref _PinViewModelList, value); }
        }

        List<Pin> _pinViewModelSearchBarList;
        public List<Pin> PinViewModelSearchBarList
        {
            get { return _pinViewModelSearchBarList; }
            set { SetProperty(ref _pinViewModelSearchBarList, value); }
        }

        private bool _IsInfoVisible;
        public bool IsInfoVisible
        {
            get { return _IsInfoVisible; }
            set { SetProperty(ref _IsInfoVisible, value); }
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

        private double _latitudeSelectedPin;
        public double LatitudeSelectedPin
        {
            get { return _latitudeSelectedPin; }
            set { SetProperty(ref _latitudeSelectedPin, value); }
        }
        private string _addressSelectedPin;
        public string AddressSelectedPin
        {
            get { return _addressSelectedPin; }
            set { SetProperty(ref _addressSelectedPin, value); }
        }

        private string _labelSelectedPin;
        public string LabelSelectedPin
        {
            get { return _labelSelectedPin; }
            set { SetProperty(ref _labelSelectedPin, value); }
        }

        private Pin _SelectedPin;
        public Pin SelectedPin
        {
            get { return _SelectedPin; }
            set { SetProperty(ref _SelectedPin, value); }
        }

        private PinViewModel _PinViewModel;
        public PinViewModel PinViewModell
        {
            get { return _PinViewModel; }
            set { SetProperty(ref _PinViewModel, value); }
        }

        private bool _myLocationButtonVisibility = false;
        public bool MyLocationButtonVisibility
        {
            get { return _myLocationButtonVisibility; }
            set { SetProperty(ref _myLocationButtonVisibility, value); }
        }

        private ObservableCollection <PinViewModel> _PlacesList;
        public ObservableCollection <PinViewModel> PlacesList
        {
            get { return _PlacesList; }
            set { SetProperty(ref _PlacesList, value); }
        }

        private ICommand _SaveLastPositionAfterMoveCommand;
        public ICommand SaveLastPositionAfterMoveCommand => _SaveLastPositionAfterMoveCommand ?? new Command(OnSaveLastPosition);


        private ICommand _NavigationToMainPage;
        public ICommand NavigationToMainPage => _NavigationToMainPage ?? new Command(OnNavigationToMainPage);


        private ICommand _SavePositionCommand;
        public ICommand SavePositionCommand => _SavePositionCommand ?? new Command(SaveDataCameraPosition);


        private ICommand _CloseCommand;
        public ICommand CloseCommand => _CloseCommand ?? new Command(OnMakeFormInactive);


        private ICommand _TextChangedCommand;
        public ICommand TextChangedCommand => _TextChangedCommand ?? new Command(OnTextChangedCommand);


        private ICommand _ListItemTapCommand;
        public ICommand ListItemTapCommand => _ListItemTapCommand ?? new Command(OnListItemTapCommand);


        private ICommand _NavigationToSettingsView;
        public ICommand NavigationToSettingsView => _NavigationToSettingsView ?? (_NavigationToSettingsView = new Command(OnNavigationToSettingsView));


        private ICommand _BackTapCommand;
        public ICommand BackTapCommand => _BackTapCommand ?? new Command(OnBackTapCommand);


        private ICommand _MapClickCommand;
        public ICommand MapClickCommand => _MapClickCommand ?? new Command(OnMapClickCommand);


        private ICommand _PinClickedCommand;
        public ICommand PinClickedCommand => _PinClickedCommand?? new Command<Pin>(OnPinClick);

        private ICommand _TapClockButtonCommand;
        public ICommand TapClockButtonCommand => _TapClockButtonCommand ?? new Command(OnTapClockButton);


        #endregion

        #region---Methods---

        private async void OnTapClockButton()
        {
            //var parametr = new NavigationParameters();
            //parametr.Add(ListOfConstants.SelectedPin, SelectedPin);

            _timeZoneService.GetCurrentTime(SelectedPin.Position);
            await _navigationService.NavigateAsync($"{ nameof(ClockView)}", useModalNavigation:true, animated: true);
        }

        private void OnPerformSearchCommand(object parametr)
        {

        }

        private void OnMapClickCommand()
        {
            if (IsVisibleListView|| IsInfoVisible)
            {
                IsVisibleListView = false;
                IsInfoVisible = false;
            }
        }

        private void OnBackTapCommand(object obj)
        {
            ExitSearch = true;
        }
        private async void OnNavigationToSettingsView()
        {
            await _navigationService.NavigateAsync($"{ nameof(SettingsView)}");
        }
        public IEnumerable<PinViewModel> ConvertingPinModelToPinViewModel(IEnumerable<PinModel> PinModellist)
        {
            var pinViewModelList = new ObservableCollection<PinViewModel>();
            foreach (var pinModel in PinModellist)
            {
                var convertingPinViewModel = pinModel.ToPinViewModel();
                if (convertingPinViewModel != null)
                {
                    pinViewModelList.Add(convertingPinViewModel);
                }
            }
            return pinViewModelList;
        }

        private async void OnNavigationToMainPage()
        {
            LoggingOutUser();
            await _navigationService.NavigateAsync($"/{nameof(MainPage)}");
        }
        private void LoggingOutUser() //When logging out, delete all user settings
        {
            _authorizationService.Unauthorize();
        }
        private void OnListItemTapCommand(object parametr)
        {
            PinViewModell = parametr as PinViewModel;
            if (PinViewModell != null)
            {
                IsVisibleListView = false;
                ExitSearch = true;
                MovingCameraPosition = new Position(PinViewModell.Latitude, PinViewModell.Longitude);
            }
        }
        private async void OnTextChangedCommand(object parametr)
        {
            string result = parametr.ToString();
            if (!string.IsNullOrWhiteSpace(result))
            {
                var resultGetPins = await _pinServices.GetPinListAsync(result);
                if (resultGetPins.Count != 0 && resultGetPins != null)
                {
                    if(ListOfConstants.ItemVisibleElementOfListView>= resultGetPins.Count)
                    {
                        SizeHightListView = resultGetPins.Count * SizeRow;
                    }
                    else
                    {
                        SizeHightListView =ListOfConstants.ItemVisibleElementOfListView * SizeRow;
                    }
                    IsVisibleListView = true;
                    var profileViewModelList = ConvertingPinModelToPinViewModel(resultGetPins);
                    PlacesList = (ObservableCollection<PinViewModel>)profileViewModelList;
                }
                else
                {
                    IsVisibleListView = false;
                }
            }
            else
            {
                IsVisibleListView = false;
            }
            //Save state of text
        }

        private void OnSaveLastPosition(object itemObject)
        {
            CameraPosition cameraPosition = itemObject as CameraPosition;
            if(cameraPosition!=null)
            {
                _cameraService.SaveDataCameraPosition(cameraPosition);
            }
        }

        private void SaveDataCameraPosition(object item)
        {
            Position position = (Position)item;
            if(position != null)
            {

            }
        }

        private void OnMakeFormInactive()
        {
            IsInfoVisible = false;
            //await _navigationService.GoBackAsync();
        }

        private async void OnImagePreview()
        {
            (ImagePinViewModel, ObservableCollection<ImagePinViewModel>) imagePinData = (SelectedImage, ImagePinViewModels1);
            var parameters = new NavigationParameters();

            parameters.Add(ListOfConstants.SelectedImage, imagePinData);
            await _navigationService.NavigateAsync(nameof(PhotoView), parameters, useModalNavigation: true);
        }

        private async void OnPinClick(object parametr)
        {
            Pin pin = parametr as Pin;

            if(pin!=null)
            {
                SelectedPin = pin;

                var resultPinModel = await _pinServices.GetPinListAsync(pin.Label);
                var resultImagesPin = await _imagesPinService.GetAllImagePinModelAsync(resultPinModel[0].Id);  //TO DO

                if (resultPinModel.Count != 0)
                {
                    var resultPinViewModel = resultPinModel[0].ToPinViewModel();

                    if (resultImagesPin != null)
                    {
                        ImagePinViewModels1 = new ObservableCollection<ImagePinViewModel>();
                        foreach (ImagesPin imagesPins in resultImagesPin)
                        {
                            ImagePinViewModels1.Add(imagesPins.ToImagePinViewModel());
                        }
                    }

                    IsInfoVisible = true;
                    LabelSelectedPin = resultPinViewModel.Label;
                    AddressSelectedPin = resultPinViewModel.Address;
                    LatitudeSelectedPin = resultPinViewModel.Latitude;
                    LongitudeSelectedPin = resultPinViewModel.Longitude;
                    DescriptionSelectedPin = resultPinViewModel.Description;

                    if (ImagePinViewModels1.Count != 0)
                    {
                        IsVisibleCollectionView = true;
                    }
                    else
                    {
                        IsVisibleCollectionView = false;
                    }
                    /*
                    if (resultPinModel.Count!=0)
                    {
                        var resultPinViewModel= resultPinModel[0].ToPinViewModel();

                        ObservableCollection<ImagePinViewModel> imagePinViewModels = null;
                        if(resultImagesPin!=null)
                        {
                            imagePinViewModels=new ObservableCollection<ImagePinViewModel>();
                            foreach (ImagesPin imagesPins in resultImagesPin)
                            {
                                imagePinViewModels.Add(imagesPins.ToImagePinViewModel());
                            }
                        }

                        var parametr1 = new NavigationParameters();
                        (ObservableCollection <ImagePinViewModel>, PinViewModel) pinData = (imagePinViewModels, resultPinViewModel);
                        parametr1.Add(ListOfConstants.SelectedPinData, pinData);
                        await _navigationService.NavigateAsync(nameof(PopupView), parametr1, useModalNavigation: true, animated: true);
                    */

                    //IsInfoVisible = true;
                    //LabelSelectedPin = resultPinModel[0].Label;
                    //AddressSelectedPin = resultPinModel[0].Address;
                    //LatitudeSelectedPin = resultPinModel[0].Latitude;
                    //LongitudeSelectedPin = resultPinModel[0].Longitude;
                    //DescriptionSelectedPin = resultPinModel[0].Description;

                }
            }
            /*
            Pin pin = parametr as Pin;
            if (pin != null)
            {
                Pin = pin;
                IsInfoVisible = true;
                LabelSelectedPin = Pin.Label;
                AddressSelectedPin = Pin.Address;
                LatitudeSelectedPin = Pin.Position.Latitude;
                LongitudeSelectedPin = Pin.Position.Longitude;

               // DescriptionSelectedPin=
            }
            */
        }

        public ICommand IsVisableCommand => new Command(EnabledMyLocation);
        private async void RequestLocationPermission()
        {
            bool result = await _permissionService.GetPermissionAsync(Permission.Location);
            if(result&&!_permissionService.GetStatusPermission())
            {
                _permissionService.SetStatusPermission(result);
            }
        }
        private async void EnabledMyLocation()
        {

            if (MyLocationButtonVisibility!=true)
            {
                bool result = await _permissionService.GetPermissionAsync(Permission.Location);
                if(result==true)
                {
                    _permissionService.SetStatusPermission(result);
                    MyLocationButtonVisibility = result;
                    //PathImageOfLocation = "icons_mylocation_on.png"; 
                }
            }
            else
            {
                MyLocationButtonVisibility = false;
               //PathImageOfLocation = "icons_mylocation_off.png";
            }
        }
        private async void GetAllPins()
        {
            var result = await _pinServices.GetPinListAsync();
            List<Pin> pins = new List<Pin>();
            if (result!=null&& result.Count()!=0)
            {
                //pins = new List<Pin>();
                foreach (PinModel pinModel in result)
                {
                    Pin result1 = pinModel.ToPin();
                    pins.Add(result1);
                }
                PinViewModelList=pins;
            }
            else
            {
                PinViewModelList = pins;
            }
        }
        /*
        private void AddPinToMap()
        {
            List<Pin> pins2 = new List<Pin>()
            {
            new Pin
            {
                Label = "Santa Cruz",
                Address = "The city with a boardwalk",
                //Type = PinType.Place,
                Position = new Position(10.9628066, -100.0194722)
            },
            new Pin
            {
                Label = "Supermarket",
                Address = "The city with a boardwalk",
                //Type = PinType.Place,
                Position = new Position(10.9628066, -150.0194722)
            },
            new Pin
            {
                Label = "Department store",
                Address = "Cooking",
                //Type = PinType.Place,
                Position = new Position(50.9628066, -200.0194722)
            }
        };
            PinViewModelList = pins2;
        }
        */

        #endregion
        public void Initialize(INavigationParameters parameters)
        {
            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(_cameraService.GetDataCameraPosition());
            bool resultPremission = _permissionService.GetStatusPermission();
            if (resultPremission)
            {
                MyLocationButtonVisibility = resultPremission;
            }
            else
            {
                MyLocationButtonVisibility = resultPremission;
            }
        }

        #region--Iterface INavigatedAware implementation--
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            GetAllPins();
            if (parameters.TryGetValue<PinViewModel>(ListOfConstants.SelectedPin, out PinViewModel pinViewModel))
            {
                PinViewModell = parameters.GetValue<PinViewModel>(ListOfConstants.SelectedPin);
                if(PinViewModell!=null)
                {
                    
                    MovingCameraPosition = new Position(PinViewModell.Latitude, PinViewModell.Longitude);
                }
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            if(IsInfoVisible)
            {
                IsInfoVisible = false;
            }
        }
        #endregion
        private async void ShowRelevantPins()
        {
            if(!string.IsNullOrWhiteSpace(SearchText))
            {
                var resultGetPins = await _pinServices.GetPinListAsync(SearchText);
                if (resultGetPins.Count != 0 && resultGetPins != null)
                {
                    if (ListOfConstants.ItemVisibleElementOfListView >= resultGetPins.Count)
                    {
                        SizeHightListView = resultGetPins.Count * SizeRow;
                    }
                    else
                    {
                        SizeHightListView = ListOfConstants.ItemVisibleElementOfListView * SizeRow;
                    }
                    IsVisibleListView = true;
                    var profileViewModelList = ConvertingPinModelToPinViewModel(resultGetPins);
                    PlacesList = (ObservableCollection<PinViewModel>)profileViewModelList;
                }
                else
                {
                    IsVisibleListView = false;
                }
            }
            else
            {
                IsVisibleListView = false;
            }
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(SearchText))
            {
                ShowRelevantPins();
            }
            if (args.PropertyName == nameof(SelectedImage))
            {
                OnImagePreview();
            }
        }
    }
}

