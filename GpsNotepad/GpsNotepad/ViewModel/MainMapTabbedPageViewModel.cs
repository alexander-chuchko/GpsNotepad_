﻿using GpsNotepad.Model.Pin;
using GpsNotepad.Services.Camera;
using GpsNotepad.Services.Pin;
using Plugin.Geolocator;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
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
        private CameraUpdate _initialCameraUpdate;
        private bool _myLocationButtonVisibility = false;
        private string _pathImageOfLocation = "icons_mylocation_off.png";

        public MainMapTabbedPageViewModel(INavigationService navigationService, ICameraService cameraService, IPinServices pinServices, IPermissionService permissionService) : base(navigationService)
        {
            _cameraService = cameraService;
            _pinServices= pinServices;
            _permissionService = permissionService;
            GetAllPins();
            //AddPinToMap();
            NavigationToMainList = new Command(ExecuteGoToMainList);
            SaveLastPositionAfterMoveCommand = new Command<CameraPosition>(SaveLastPosition);
            InitialCameraUpdate = CameraUpdateFactory.NewPosition(new Position(0, 0));
        }
        public ICommand SaveLastPositionAfterMoveCommand { get; set; }
        public ICommand NavigationToMainList { get; set; }
        public ICommand SavePositionCommand => new Command(SaveDataCameraPosition);
        public ICommand CloseCommand => new Command(MakeFormInactive);

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

        private async void ExecuteGoToMainList()
        {
            //await _navigationService.GoBackAsync();
            await _navigationService.NavigateAsync($"{ nameof(NavigationPage)}/{ nameof(MainListTabbedPageView)}");
        }
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
        private void OnPinClick(object selectedObject)
        {
            Pin pin = selectedObject as Pin;
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
        private async void EnabledMyLocation()
        {
            if(MyLocationButtonVisibility!=true)
            {
                bool result = await _permissionService.GetPermissionAsync(Permission.Location);
                MyLocationButtonVisibility = result;
                PathImageOfLocation = "icons_mylocation_on.png";
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
        public void Initialize(INavigationParameters parameters)
        {
            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(_cameraService.GetDataCameraPosition());
        }
        #region--Iterface INavigatedAware implementation--
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinViewModel>(ListOfNames.selectedPin, out PinViewModel pinViewModel))
            {
                PinViewModell = parameters.GetValue<PinViewModel>(ListOfNames.selectedPin);
                MovingCameraPosition = new Position(PinViewModell.Latitude, PinViewModell.Longitude);
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }
        #endregion
    }
}
