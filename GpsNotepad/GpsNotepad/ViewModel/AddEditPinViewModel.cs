using GpsNotepad.Model.Pin;
using GpsNotepad.Service.Authorization;
using Prism.Navigation;
using GpsNotepad.Services.Pin;
using Xamarin.Forms;
using System.Windows.Input;
using GpsNotepad.Helpers;
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
using System.ComponentModel;
using GpsNotepad.Model.ImagePin;
using System.Collections.ObjectModel;
using GpsNotepad.Services.ImagesOfPin;
using Acr.UserDialogs;
using GpsNotepad.Services.Media;
using System.IO;

namespace GpsNotepad.ViewModel
{
    public class AddEditPinViewModel:BaseViewModel, INavigatedAware, IInitialize
    {
        #region   ---   PrivateFields   ---

        private readonly IPinServices _pinServices;
        private readonly IPermissionService _permissionService;
        private readonly ICameraService _cameraService;
        private readonly IImagesPinService _imagesPinService;
        private readonly IMediaService _mediaService;

        #endregion

        public AddEditPinViewModel(INavigationService navigationService,
            IAuthorizationService authorizationService,
            IPinServices pinServices,
            IPageDialogService pageDialogService,
            IPermissionService permissionService,
            ICameraService cameraService, IImagesPinService imagesPinService, IMediaService mediaService) :base(navigationService)
        {
            LabelOfPin = string.Empty;
            Description = string.Empty;
            Longitude = string.Empty;
            Latitude = string.Empty;
            IsEnable = false;
            _mediaService = mediaService;
            _imagesPinService = imagesPinService;
            _pinServices = pinServices;
            _permissionService= permissionService;
            _cameraService = cameraService;
            GetAllPins();
            InitialCameraUpdate = CameraUpdateFactory.NewPosition(new Position(0, 0));
            OpenGallery = OnGetPathForImageFromGallery;
            TakePhoto = OnGetPathForImageFromCamera;
        }

        #region    ---   PublicProperties   ---

        public Action OpenGallery { get; set; }
        public Action TakePhoto { get; set; }


        private bool _IsEnableIconZoom=true;
        public bool IsEnableIconZoom
        {
            get { return _IsEnableIconZoom; }
            set { SetProperty(ref _IsEnableIconZoom, value); }
        }


        private ObservableCollection<ImagePinViewModel> _ImagesPin;
        public ObservableCollection<ImagePinViewModel> ImagesPin
        {
            get { return _ImagesPin; }
            set { SetProperty(ref _ImagesPin, value); }
        }


        private string _LabelOfPin;
        public string LabelOfPin
        {
            get { return _LabelOfPin; }
            set { SetProperty(ref _LabelOfPin, value); }
        }

        private string _ImageSourceForLabel = ListOfConstants.ButtonClear;
        public string ImageSourceForLabel
        {
            get { return _ImageSourceForLabel; }
            set { SetProperty(ref _ImageSourceForLabel, value); }
        }


        private string _ErrorLabel = string.Empty;
        public string ErrorLabel
        {
            get { return _ErrorLabel; }
            set { SetProperty(ref _ErrorLabel, value); }
        }


        private string _PlaceholderForLabel = ListOfConstants.PlaceholderLabel;
        public string PlaceholderForLabel
        {
            get { return _PlaceholderForLabel; }
            set { SetProperty(ref _PlaceholderForLabel, value); }
        }


        private bool _IsTapedImageOfLabel;
        public bool IsTapedImageOfLabel
        {
            get { return _IsTapedImageOfLabel; }
            set { SetProperty(ref _IsTapedImageOfLabel, value); }
        }


        private Color _LabelBorderColor = Color.LightGray;
        public Color LabelBorderColor
        {
            get { return _LabelBorderColor; }
            set { SetProperty(ref _LabelBorderColor, value); }
        }


        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value); }
        }


        private string _ImageSourceForDescription = ListOfConstants.ButtonClear;
        public string ImageSourceForDescription
        {
            get { return _ImageSourceForDescription; }
            set { SetProperty(ref _ImageSourceForDescription, value); }
        }


        private string _ErrorDescription = string.Empty;
        public string ErrorDescription
        {
            get { return _ErrorDescription; }
            set { SetProperty(ref _ErrorDescription, value); }
        }


        private string _PlaceholderForDescription = ListOfConstants.PlaceholderDescription;
        public string PlaceholderForDescription
        {
            get { return _PlaceholderForDescription; }
            set { SetProperty(ref _PlaceholderForDescription, value); }
        }


        private bool _IsTapedImageOfDescription;
        public bool IsTapedImageOfDescription
        {
            get { return _IsTapedImageOfDescription; }
            set { SetProperty(ref _IsTapedImageOfDescription, value); }
        }


        private Color _DescriptionBorderColor = Color.LightGray;
        public Color DescriptionBorderColor
        {
            get { return _DescriptionBorderColor; }
            set { SetProperty(ref _DescriptionBorderColor, value); }
        }


        private string _Longitude;
        public string Longitude
        {
            get { return _Longitude; }
            set { SetProperty(ref _Longitude, value); }
        }


        private string _ImageSourceForLongitude = ListOfConstants.ButtonClear;
        public string ImageSourceForLongitude
        {
            get { return _ImageSourceForLongitude; }
            set { SetProperty(ref _ImageSourceForLongitude, value); }
        }


        private string _ErrorLongitude = string.Empty;
        public string ErrorLongitude
        {
            get { return _ErrorLongitude; }
            set { SetProperty(ref _ErrorLongitude, value); }
        }


        private string _PlaceholderForLongitude = ListOfConstants.PlaceholderLongitude;
        public string PlaceholderForLongitude
        {
            get { return _PlaceholderForLongitude; }
            set { SetProperty(ref _PlaceholderForLongitude, value); }
        }


        private bool _IsTapedImageOfLongitude;
        public bool IsTapedImageOfLongitude
        {
            get { return _IsTapedImageOfLongitude; }
            set { SetProperty(ref _IsTapedImageOfLongitude, value); }
        }


        private Color _LongitudeBorderColor = Color.LightGray;
        public Color LongitudeBorderColor
        {
            get { return _LongitudeBorderColor; }
            set { SetProperty(ref _LongitudeBorderColor, value); }
        }


        private string _Latitude;
        public string Latitude
        {
            get { return _Latitude; }
            set { SetProperty(ref _Latitude, value); }
        }


        private string _ImageSourceForLatitude = ListOfConstants.ButtonClear;
        public string ImageSourceForLatitude
        {
            get { return _ImageSourceForLatitude; }
            set { SetProperty(ref _ImageSourceForLatitude, value); }
        }


        private string _ErrorLatitude = string.Empty;
        public string ErrorLatitude
        {
            get { return _ErrorLatitude; }
            set { SetProperty(ref _ErrorLatitude, value); }
        }


        private string _PlaceholderForLatitude = ListOfConstants.PlaceholderLatitude;
        public string PlaceholderForLatitude
        {
            get { return _PlaceholderForLatitude; }
            set { SetProperty(ref _PlaceholderForLatitude, value); }
        }


        private bool _IsTapedImageOfLatitude;
        public bool IsTapedImageOfLatitude
        {
            get { return _IsTapedImageOfLatitude; }
            set { SetProperty(ref _IsTapedImageOfLatitude, value); }
        }


        private Color _LatitudeBorderColor = Color.LightGray;
        public Color LatitudeBorderColor
        {
            get { return _LatitudeBorderColor; }
            set { SetProperty(ref _LatitudeBorderColor, value); }
        }


        private List<Pin> _PinViewModelList;
        public List<Pin> PinViewModelList
        {
            get { return _PinViewModelList; }
            set { SetProperty(ref _PinViewModelList, value); }
        }


        private bool _MyLocationButtonVisibility;
        public bool MyLocationButtonVisibility
        {
            get { return _MyLocationButtonVisibility; }
            set { SetProperty(ref _MyLocationButtonVisibility, value); }
        }


        private CameraUpdate _InitialCameraUpdate;
        public CameraUpdate InitialCameraUpdate
        {
            get { return _InitialCameraUpdate; }
            set { SetProperty(ref _InitialCameraUpdate, value); }
        }


        private bool _IsEnable;
        public bool IsEnable
        {
            get { return _IsEnable; }
            set { SetProperty(ref _IsEnable, value); }
        }


        private PinViewModel _PinViewModel;
        public PinViewModel PinViewModel
        {
            get { return _PinViewModel; }
            set { SetProperty(ref _PinViewModel, value); }
        }


        private Position _MovingCameraPosition;
        public Position MovingCameraPosition
        {
            get { return _MovingCameraPosition; }
            set { SetProperty(ref _MovingCameraPosition, value); }
        }


        private bool _IsVisibleListPicture;
        public bool IsVisibleListPicture
        {
            get { return _IsVisibleListPicture; }
            set { SetProperty(ref _IsVisibleListPicture, value); }
        }


        private int _SizeRow = ListOfConstants.HeightRowForAddPage;
        public int SizeRow
        {
            get { return _SizeRow; }
            set { SetProperty(ref _SizeRow, value); }
        }


        private int _SizeHightListView;
        public int SizeHightListView
        {
            get { return _SizeHightListView; }
            set { SetProperty(ref _SizeHightListView, value); }
        }


        private ICommand _RemovePictureCommand;
        public ICommand RemovePictureCommand => _RemovePictureCommand ?? new Command(OnRemovePicture);


        private ICommand _AddPhotoCommand;
        public ICommand AddPhotoCommand => _AddPhotoCommand ?? new Command(OnAddPhoto);


        private ICommand _SaveCommand;
        public ICommand SaveCommand => _SaveCommand ?? new Command(OnSaveOrUpdatePinModel);


        private ICommand _MapClickCommand;
        public ICommand MapClickCommand => _MapClickCommand ?? new Command(OnMapClickCommand);


        private ICommand _NavigationToMainListCommand;
        public ICommand NavigationToMainListCommand => _NavigationToMainListCommand ?? new Command(OnNavigationToMainList);

        #endregion


        #region     ---   Methods   ---

        private void OnAddPhoto()
        {
            OnShowActionSheet();
        }

        private async void OnGetAllImagesPin()
        {
            var imagesPinAll = await _imagesPinService.GetAllImagePinModelAsync(PinViewModel.Id);

            if (imagesPinAll != null && imagesPinAll.ToList().Count != 0)
            {
                var imagePinViewModels = new ObservableCollection<ImagePinViewModel>();

                foreach (ImagesPin imagesPin in imagesPinAll)
                {
                    var imagesPinViewModel = imagesPin.ToImagePinViewModel();
                    if (imagesPinViewModel != null)
                    {
                        imagePinViewModels.Add(imagesPinViewModel);
                    }
                }
                ImagesPin = imagePinViewModels;

                IsEnableIconZoom = false;
                IsVisibleListPicture = true;

                ChangeSizeListView();
            }
            else
            {
                IsVisibleListPicture = false;
            }
        }

        private async void OnGetPathForImageFromGallery()
        {
            var pathImage = await _mediaService.GetPhotoFromGalleryAsync();

            if (pathImage != null)
            {
                OnCreateImagePin(pathImage);
            }
        }

        private async void OnGetPathForImageFromCamera()
        {
            var pathImage = await _mediaService.TakingPicturesAsync();

            if (pathImage != null)
            {
                OnCreateImagePin(pathImage);
            }
        }


        private void OnShowActionSheet()
        {
            var config = new ActionSheetConfig
            {
                Title = "Action sheet"
            };

            config.Add("Gallery", OpenGallery);
            config.Add("Camera", TakePhoto);

            ActionSheetOption cancel = new ActionSheetOption("Cancel", null, null);
            config.Cancel = cancel;

            UserDialogs.Instance.ActionSheet(config);
        }

        private async void SaveImagesPinas()
        {
            if (ImagesPin != null && ImagesPin.Count != 0)
            {
                foreach (ImagePinViewModel imagePinViewModel in ImagesPin)
                {
                    bool resultOperation = true;
                    if (imagePinViewModel.PinId == 0 && resultOperation)
                    {
                        imagePinViewModel.PinId = PinViewModel.Id;
                        var imagePinModel = imagePinViewModel.ToImagesPin();

                        if (imagePinModel != null)
                        {
                            resultOperation = await _imagesPinService.SaveImagePinModelAsync(imagePinModel);
                        }
                    }
                }
            }
        }

        private void OnCreateImagePin(string pathNewImagePin)
        {
            ImagePinViewModel imagePinViewModel = new ImagePinViewModel()
            {
                PathImage = pathNewImagePin,
                NameImage = Path.GetFileName(pathNewImagePin)
            };

            if (ImagesPin == null)
            {
                ImagesPin = new ObservableCollection<ImagePinViewModel>();
            }

            ImagesPin.Add(imagePinViewModel);

            ChangeSizeListView();

            IsVisibleListPicture = true;
            IsEnableIconZoom = false;
        }
        private void ChangeSizeListView()
        {
            if (ListOfConstants.NumberOfVisibleListViewItemsForPageAddPin >= ImagesPin.Count)
            {
                SizeHightListView = ImagesPin.Count * SizeRow;
            }
            else
            {
                SizeHightListView = ListOfConstants.NumberOfVisibleListViewItemsForPageAddPin * SizeRow;
            }
        }

        private async void OnRemovePicture(object parametr)
        {

            ImagePinViewModel imagePinViewModel = parametr as ImagePinViewModel;

            if (imagePinViewModel != null)
            {
                if (imagePinViewModel.PinId == 0)
                {
                    ImagesPin.Remove(imagePinViewModel);
                }
                else
                {
                    var imagesPin = imagePinViewModel.ToImagesPin();
                    var result = await _imagesPinService.DeleteImagePinModelAsync(imagesPin);

                    if (result)
                    {
                        ImagesPin.Remove(imagePinViewModel);
                    }
                }

                ChangeSizeListView();

                if (ImagesPin.Count == 0)
                {
                    IsVisibleListPicture = false;
                    IsEnableIconZoom = true;
                }
            }
        }

        private void OnMapClickCommand(object parametr)
        {
            Position position = (Position)parametr;
            Longitude = string.Format("{0,12:N10}", position.Longitude).ToString();
            Latitude = string.Format("{0,12:N10}", position.Latitude).ToString();
        }

        private async void OnNavigationToMainList()
        {
            await _navigationService.GoBackAsync();
        }

        private async void OnSaveOrUpdatePinModel()
        {

            var result = true;

            if (!Validation.IsValidatedLabelAndDescription(LabelOfPin) && result)
            {
                result = false;
                ErrorLabel = ListOfConstants.WrongLabel;
                LabelBorderColor = Color.Red;
            }

            if(PinViewModel==null)
            {
                var resultPickingLabels = await _pinServices.GetPinListAsync(LabelOfPin);


                if (resultPickingLabels != null && resultPickingLabels.Count > 0)
                {
                    result = false;
                    ErrorLabel = ListOfConstants.WrongExsistLabel;
                    LabelBorderColor = Color.Red;
                }
            }


            if (!Validation.IsValidatedLabelAndDescription(Description) && result)
            {
                result = false;
                ErrorDescription = ListOfConstants.WrongDescription;
                DescriptionBorderColor = Color.Red;
            }


            if (!Validation.IsValidatedLongitude(Longitude) && result)
            {
                result = false;
                ErrorLongitude = ListOfConstants.WrongLongitude;
                LongitudeBorderColor = Color.Red;
            }


            if (!Validation.IsValidatedLatitude(Latitude) && result)
            {
                result = false;
                ErrorLatitude = ListOfConstants.WrongLatitude;
                LatitudeBorderColor = Color.Red;
            }


            if (result)
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
                    SaveImagesPinas();
                    var parametr = new NavigationParameters();
                    parametr.Add(ListOfConstants.PinViewModel, PinViewModel);
                    await _navigationService.GoBackAsync(parametr);
                }
            }
        }

        private async Task<bool> UpdatePinModel()
        {
            bool resultOfAction = false;

            PinViewModel.Label = LabelOfPin;
            PinViewModel.Description = Description;
            PinViewModel.Latitude = Convert.ToDouble(Latitude);
            PinViewModel.Longitude = Convert.ToDouble(Longitude);

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

            PinViewModel = new PinViewModel()
            {
                Label = LabelOfPin,
                Description = Description,
                Latitude = Convert.ToDouble(Latitude),
                Longitude = Convert.ToDouble(Longitude),
                ImagePath = "ic_like_gray.png"
            };

            var PinModel = PinViewModel.ToPinModel();

            if (PinModel != null)
            {
                resultOfAction = await _pinServices.SaveOrUpdatePinModelToStorageAsync(PinModel);
                if(resultOfAction)
                {
                    PinViewModel.Id = PinModel.Id;
                    PinViewModel.UserId = PinModel.UserId;
                }
            }

            return resultOfAction;
        }

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

        #endregion


        #region     ---   Overrides   ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsTapedImageOfLabel))
            {
                LabelOfPin = string.Empty;
            }
            else if (args.PropertyName == nameof(IsTapedImageOfDescription))
            {
                Description = string.Empty;
            }
            else if(args.PropertyName == nameof(IsTapedImageOfLongitude))
            {
                Longitude = string.Empty;
            }
            else if(args.PropertyName == nameof(IsTapedImageOfLatitude))
            {
                Latitude = string.Empty;
            }

            if (args.PropertyName == nameof(LabelOfPin) && LabelBorderColor == Color.Red && ErrorLabel != string.Empty)
            {
                LabelBorderColor = Color.FromHex("#D7DDE8");
                ErrorLabel = string.Empty;
            }
            else if (args.PropertyName == nameof(Description) && DescriptionBorderColor == Color.Red && ErrorDescription != string.Empty)
            {
                DescriptionBorderColor = Color.FromHex("#D7DDE8");
                ErrorDescription = string.Empty;
            }
            else if(args.PropertyName == nameof(Longitude) && LongitudeBorderColor == Color.Red && ErrorLongitude != string.Empty)
            {
                LongitudeBorderColor = Color.FromHex("#D7DDE8");
                ErrorLongitude = string.Empty;
            }
            else if(args.PropertyName == nameof(Latitude) && LatitudeBorderColor == Color.Red && ErrorLatitude != string.Empty)
            {
                LatitudeBorderColor = Color.FromHex("#D7DDE8");
                ErrorLatitude = string.Empty;
            }
        }

        #endregion


        #region   ---   Iterface INavigatedAware implementation   --- 

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinViewModel>(ListOfConstants.PinModel, out PinViewModel pinViewModel))
            {
                PinViewModel = parameters.GetValue<PinViewModel>(ListOfConstants.PinModel);

                if (PinViewModel != null)
                {
                    LabelOfPin = PinViewModel.Label;
                    Description = PinViewModel.Description;
                    Longitude = PinViewModel.Longitude.ToString();
                    Latitude = PinViewModel.Latitude.ToString();
                    MovingCameraPosition = new Position(PinViewModel.Latitude, PinViewModel.Longitude);
                    OnGetAllImagesPin();
                }
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        #endregion
    }
}
