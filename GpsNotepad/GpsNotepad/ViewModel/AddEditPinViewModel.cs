using GpsNotepad.Model.Pin;
using GpsNotepad.Service.Authorization;
using Prism.Navigation;
using GpsNotepad.Services.Pin;
using Xamarin.Forms;
using System.Windows.Input;
using GpsNotepad.Helpers;
using GpsNotepad.Recource;
using Prism.Services;
using System.Threading.Tasks;
using GpsNotepad.Extension;
using System;
using Xamarin.Forms.GoogleMaps;
using GpsNotepad.Services.Permissions;
using GpsNotepad.Services.Camera;
using System.Collections.Generic;
using System.Linq;
using GpsNotepad.Model;
using System.Collections.ObjectModel;

namespace GpsNotepad.ViewModel
{
    public class AddEditPinViewModel:BaseViewModel, INavigatedAware, IInitialize
    {
        #region---PrivateFields---
        private string _label;
        private string _description;
        private string _latitude;
        private string _longitude;
        private bool _isEnable;
        private PinViewModel  pinViewModel;
        private Position _movingCameraPosition;
        private CameraUpdate _initialCameraUpdate;
        private bool _myLocationButtonVisibility;
        private List<Pin> _pinViewModelList;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPinServices _pinServices;
        private readonly IPageDialogService _pageDialogService;
        private readonly IPermissionService _permissionService;
        private readonly ICameraService _cameraService;
        #endregion
        public AddEditPinViewModel(INavigationService navigationService, IAuthorizationService authorizationService, IPinServices pinServices, IPageDialogService pageDialogService, IPermissionService permissionService, ICameraService cameraService) :base(navigationService)
        {
            Label = string.Empty;
            Description = string.Empty;
            Longitude = string.Empty;
            Latitude = string.Empty;
            IsEnable = false;
            _authorizationService = authorizationService;
            _pinServices = pinServices;
            _pageDialogService = pageDialogService;
            _permissionService= permissionService;
            _cameraService = cameraService;
            GetAllPins();
            SaveCommand = new Command(SaveOrUpdatePinModel);
            InitialCameraUpdate = CameraUpdateFactory.NewPosition(new Position(0, 0));
        }
        #region---PublicProperties---
        private ICommand _MapClickCommand;
        public ICommand MapClickCommand => _MapClickCommand ?? new Command(OnMapClickCommand);
        private ICommand _PinClickedCommand;
        public ICommand PinClickedCommand => _PinClickedCommand?? new Command<Pin>(OnPinClickedCommand);
        private ICommand _NavigationToMainListCommand;
        public ICommand NavigationToMainListCommand=> _NavigationToMainListCommand?? new Command(OnNavigationToMainList);
        private void OnPinClickedCommand(Pin parametr)
        {
            Pin pin = parametr as Pin;
            if (pin != null)
            {

            }
        }
        private void OnMapClickCommand(object parametr)
        {
            Position position = (Position)parametr;
            //MovingCameraPosition = position;
            Latitude = position.Latitude.ToString();
            Longitude = position.Longitude.ToString();
        }
        public ICommand SaveCommand { get; set; }
       

        public List<Pin> PinViewModelList
        {
            get { return _pinViewModelList; }
            set { SetProperty(ref _pinViewModelList, value); }
        }
        public bool MyLocationButtonVisibility
        {
            get { return _myLocationButtonVisibility; }
            set { SetProperty(ref _myLocationButtonVisibility, value); }
        }

        public CameraUpdate InitialCameraUpdate
        {
            get => _initialCameraUpdate;
            set => SetProperty(ref _initialCameraUpdate, value);
        }
        public bool IsEnable
        {
            get { return _isEnable; }
            set { SetProperty(ref _isEnable, value); }
        }
        public PinViewModel PinViewModel
        {
            get { return pinViewModel; }
            set { SetProperty(ref pinViewModel, value); }
        }
        public string Label
        {
            get { return _label; }
            set{ SetProperty(ref _label, value); }
        }
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        public string Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value); }
        }
        public string Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value); }
        }
        public Position MovingCameraPosition
        {
            get => _movingCameraPosition;
            set => SetProperty(ref _movingCameraPosition, value);
        }
        #endregion
        #region---Methods---
        private bool IsFieldsFilled()
        {
            bool resultFilling = true;
            if (!Validation.IsInformationInLabelAndLatitudeAndLongitude(Label, Latitude, Longitude))
            {
                resultFilling = false;
            }
            return resultFilling;
        }
        private async void OnNavigationToMainList()
        {
            await _navigationService.GoBackAsync();
        }
        private async void SaveOrUpdatePinModel()
        {
            if (IsFieldsFilled())
            {
                bool resultOfAction = false;
                if (PinViewModel != null)
                {
                    resultOfAction = await UpdatePinModel();
                }
                else
                {
                    resultOfAction = await AddPinModel();
                }
                if (resultOfAction)
                {
                    var parametr = new NavigationParameters();
                    parametr.Add(ListOfNames.PinViewModel, PinViewModel);
                    await _navigationService.GoBackAsync(parametr);
                }
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync(AppResource.information_is_missing_in_the_fields_label_and_latitude_and_longitude, AppResource.invalid_data_entered, "OK");
            }
        }
        private async Task<bool> UpdatePinModel()
        {
            bool resultOfAction = false;
            PinViewModel.Label = Label;
            PinViewModel.Description = Description;
            PinViewModel.Latitude =Convert.ToDouble(Latitude);//to do
            PinViewModel.Longitude =Convert.ToDouble(Longitude);//to do
            var pinModel = PinViewModel.ToPinModel();
            if (pinModel != null)
            {
                resultOfAction = await _pinServices.SaveOrUpdatePinModelToStorageAsync(pinModel);
            }
            return resultOfAction;
        }
        private async void GetAllPins()
        {
            var result = await _pinServices.GetPinListAsync();
            if (result != null && result.Count() != 0)
            {
                List<Pin> pins = new List<Pin>();
                foreach (PinModel pinModel in result)
                {
                    Pin result1 = pinModel.ToPin();
                    pins.Add(result1);
                }
                PinViewModelList = pins;
            }
        }
        private async Task<bool> AddPinModel()
        {
            bool resultOfAction = false;
            PinViewModel=new PinViewModel()
            {
                Label=Label,
                Description = Description,
                Latitude=Convert.ToDouble(Latitude),
                Longitude=Convert.ToDouble(Longitude)
            };
            var PinModel = PinViewModel.ToPinModel();
            if (PinModel != null)
            {
                resultOfAction = await _pinServices.SaveOrUpdatePinModelToStorageAsync(PinModel);
                if(resultOfAction)
                {
                    PinViewModel.Id = PinModel.Id;
                }
            }
            return resultOfAction;
        }
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
            if (parameters.TryGetValue<PinViewModel>(ListOfNames.PinModel, out PinViewModel pinViewModel))
            {
                PinViewModel = parameters.GetValue<PinViewModel>(ListOfNames.PinModel);
                if (PinViewModel != null)
                {
                    Label = PinViewModel.Label;
                    Description = PinViewModel.Description;
                    Longitude = PinViewModel.Longitude.ToString(); //No forget to correct!
                    Latitude = PinViewModel.Latitude.ToString();  //No forget to correct!
                    MovingCameraPosition = new Position(PinViewModel.Latitude, PinViewModel.Longitude);
                }
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }
        #endregion
    }
}
