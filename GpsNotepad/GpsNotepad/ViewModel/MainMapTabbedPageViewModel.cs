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

namespace GpsNotepad.ViewModel
{
    public class MainMapTabbedPageViewModel: BaseViewModel, INavigatedAware, IInitialize
    {
        private List<Pin> _pinViewModelList;
        private bool _isInfoVisible;
        private string _labelSelectedPin;
        private string _addressSelectedPin;
        private double _latitudeSelectedPin;
        private double _longitudeSelectedPin;
        private PinViewModel _pinViewModel;
        private Position _movingCameraPosition;
        private Pin _pin;
        private readonly IPinServices _pinServices;
        private readonly ICameraService _cameraService;
        private readonly IPermissionService _permissionService;
        private readonly IAuthorizationService _authorizationService;
        private CameraUpdate _initialCameraUpdate;
        private bool _myLocationButtonVisibility = false;
        private string _pathImageOfLocation = "icons_mylocation_off.png";
        private bool _isVisibleCommand;
        private bool _isVisibleScrollView;
        const int ItemVisibleElementOfListView= 5;
        private int _sizeRow=50;
        private int _sizeHightListView;

        public int SizeHightListView
        {
            get => _sizeHightListView;
            set => SetProperty(ref _sizeHightListView, value);
        }
        public int SizeRow
        {
            get => _sizeRow;
            set => SetProperty(ref _sizeRow, value);
        }
        public MainMapTabbedPageViewModel(INavigationService navigationService, ICameraService cameraService, IPinServices pinServices, IPermissionService permissionService, IAuthorizationService authorizationService) : base(navigationService)
        {
            _authorizationService = authorizationService;
            _cameraService = cameraService;
            _pinServices= pinServices;
            _permissionService = permissionService;
            GetAllPins();
            //NavigationToMainList = new Command(ExecuteGoToMainList);
            SaveLastPositionAfterMoveCommand = new Command<CameraPosition>(SaveLastPosition);
            InitialCameraUpdate = CameraUpdateFactory.NewPosition(new Position(0, 0));
            Task.Run(() => RequestLocationPermission());
            IsVisibleCommand = false;
            SizeRow = ListOfNames.HeightRow;
        }
        public ICommand SaveLastPositionAfterMoveCommand { get; set; }
        //public ICommand NavigationToMainList { get; set; }
        public ICommand NavigationToSignIn => new Command(OnNavigationToSignIn);
        public ICommand SavePositionCommand => new Command(SaveDataCameraPosition);
        public ICommand CloseCommand => new Command(MakeFormInactive);
        public ICommand PerformSearchCommand => new Command(OnPerformSearchCommand);
        public ICommand TextChangedCommand => new Command(OnTextChangedCommand);
        public ICommand ListItemTapCommand => new Command(OnListItemTapCommand);
        private ICommand _NavigationToSettingsView;
        public ICommand NavigationToSettingsView => _NavigationToSettingsView ??(_NavigationToSettingsView = new Command(OnNavigationToSettingsView));
        
        private ICommand _BackTapCommand;
        public ICommand BackTapCommand => _BackTapCommand ?? (_BackTapCommand = new Command(OnBackTapCommand));

        private void OnBackTapCommand(object obj)
        {
            ExitSearch = true;
        }
        private async void OnNavigationToSettingsView()
        {
            await _navigationService.NavigateAsync($"{ nameof(SettingsView)}");
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

        public bool IsVisibleScrollView
        {
            get => _isVisibleScrollView;
            set => SetProperty(ref _isVisibleScrollView, value);
        }
        public bool IsVisibleCommand
        {
            get => _isVisibleCommand;
            set => SetProperty(ref _isVisibleCommand, value);
        }
        public CameraUpdate InitialCameraUpdate
        {
            get => _initialCameraUpdate;
            set => SetProperty(ref _initialCameraUpdate, value);
        }
        public Position MovingCameraPosition
        {
            get => _movingCameraPosition;
            set => SetProperty(ref _movingCameraPosition, value);
        }
        public string PathImageOfLocation
        {
            get { return _pathImageOfLocation; }
            set { SetProperty(ref _pathImageOfLocation, value); }
        }
        public List<Pin> PinViewModelList
        {
            get { return _pinViewModelList; }
            set { SetProperty(ref _pinViewModelList, value); }
        }

        List<Pin> _pinViewModelSearchBarList;
        public List<Pin> PinViewModelSearchBarList
        {
            get { return _pinViewModelSearchBarList; }
            set { SetProperty(ref _pinViewModelSearchBarList, value); }
        }
        public bool IsInfoVisible
        {
            get { return _isInfoVisible; }
            set { SetProperty(ref _isInfoVisible, value); }
        }
        public double LongitudeSelectedPin
        {
            get { return _longitudeSelectedPin; }
            set { SetProperty(ref _longitudeSelectedPin, value); }
        }
        public double LatitudeSelectedPin
        {
            get { return _latitudeSelectedPin; }
            set { SetProperty(ref _latitudeSelectedPin, value); }
        }
        public string AddressSelectedPin
        {
            get { return _addressSelectedPin; }
            set { SetProperty(ref _addressSelectedPin, value); }
        }
        public string LabelSelectedPin
        {
            get { return _labelSelectedPin; }
            set { SetProperty(ref _labelSelectedPin, value); }
        }
        public Pin Pin
        {
            get { return _pin; }
            set { SetProperty(ref _pin, value); }
        }
        public PinViewModel PinViewModell
        {
            get { return _pinViewModel; }
            set { SetProperty(ref _pinViewModel, value); }
        }
        public bool MyLocationButtonVisibility
        {
            get { return _myLocationButtonVisibility; }
            set { SetProperty(ref _myLocationButtonVisibility, value); }
        }

        private ObservableCollection <PinViewModel> _placesList;
        public ObservableCollection <PinViewModel> PlacesList
        {
            get { return _placesList; }
            set { SetProperty(ref _placesList, value); }
        }
        private void OnPerformSearchCommand(object parametr)
        {

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

        private async void OnNavigationToSignIn()
        {
            DeletingCurrentUserSettings();
            //await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInView)}");
            await _navigationService.NavigateAsync($"/{nameof(MainPage)}");
        }
        private void DeletingCurrentUserSettings() //When logging out, delete all user settings
        {
            _authorizationService.Unauthorize();
        }
        private void OnListItemTapCommand(object parametr)
        {
            PinViewModell = parametr as PinViewModel;
            if (PinViewModell != null)
            {
                IsVisibleCommand = false;
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
                    if(ItemVisibleElementOfListView>= resultGetPins.Count)
                    {
                        SizeHightListView = resultGetPins.Count * SizeRow;
                    }
                    else
                    {
                        SizeHightListView = ItemVisibleElementOfListView * SizeRow;
                    }
                    IsVisibleCommand = true;
                    var profileViewModelList = ConvertingPinModelToPinViewModel(resultGetPins);
                    PlacesList = (ObservableCollection<PinViewModel>)profileViewModelList;
                }
                else
                {
                    IsVisibleCommand = false;
                }
            }
            else
            {
                IsVisibleCommand = false;
            }
            //Save state of text
        }
        /*
        private async void ExecuteGoToMainList()
        {
            //await _navigationService.GoBackAsync();
            await _navigationService.NavigateAsync($"{ nameof(NavigationPage)}/{ nameof(MainListTabbedPageView)}");
        }*/
        private void SaveLastPosition(object itemObject)
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
        private void MakeFormInactive()
        {
            IsInfoVisible = false;
        }
        public ICommand PinClickedCommand => new Command<Pin>(OnPinClick);
        private void OnPinClick(object parametr)
        {
            Pin pin = parametr as Pin;
            if (pin != null)
            {
                Pin = pin;
                IsInfoVisible = true;
                LabelSelectedPin = Pin.Label;
                AddressSelectedPin = Pin.Address;
                LatitudeSelectedPin = Pin.Position.Latitude;
                LongitudeSelectedPin = Pin.Position.Longitude;
            }
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
                    PathImageOfLocation = "icons_mylocation_on.png"; 
                }
            }
            else
            {
                MyLocationButtonVisibility = false;
                PathImageOfLocation = "icons_mylocation_off.png";
            }
        }
        private async void GetAllPins()
        {
            var result = await _pinServices.GetPinListAsync();
            if (result!=null&& result.Count()!=0)
            {
                List<Pin> pins = new List<Pin>();
                foreach (PinModel pinModel in result)
                {
                    Pin result1 = pinModel.ToPin();
                    pins.Add(result1);
                }
                PinViewModelList=pins;
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
        public void Initialize(INavigationParameters parameters)
        {
            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(_cameraService.GetDataCameraPosition());
            bool resultPremission = _permissionService.GetStatusPermission();
            if (resultPremission)
            {
                MyLocationButtonVisibility = resultPremission;
                PathImageOfLocation = "icons_mylocation_on.png";
            }
            else
            {
                MyLocationButtonVisibility = resultPremission;
                PathImageOfLocation = "icons_mylocation_off.png";
            }
        }
        #region--Iterface INavigatedAware implementation--
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinViewModel>(ListOfNames.SelectedPin, out PinViewModel pinViewModel))
            {
                PinViewModell = parameters.GetValue<PinViewModel>(ListOfNames.SelectedPin);
                if(PinViewModell!=null)
                {
                    GetAllPins(); 
                    MovingCameraPosition = new Position(PinViewModell.Latitude, PinViewModell.Longitude);
                }
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }
        #endregion
        private async void ShowRelevantPins()
        {
            if(!string.IsNullOrWhiteSpace(SearchText))
            {
                var resultGetPins = await _pinServices.GetPinListAsync(SearchText);
                if (resultGetPins.Count != 0 && resultGetPins != null)
                {
                    if (ItemVisibleElementOfListView >= resultGetPins.Count)
                    {
                        SizeHightListView = resultGetPins.Count * SizeRow;
                    }
                    else
                    {
                        SizeHightListView = ItemVisibleElementOfListView * SizeRow;
                    }
                    IsVisibleCommand = true;
                    var profileViewModelList = ConvertingPinModelToPinViewModel(resultGetPins);
                    PlacesList = (ObservableCollection<PinViewModel>)profileViewModelList;
                }
                else
                {
                    IsVisibleCommand = false;
                }
            }
            else
            {
                IsVisibleCommand = false;
            }
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(SearchText))
            {
                ShowRelevantPins();
            }
        }
    }
}
