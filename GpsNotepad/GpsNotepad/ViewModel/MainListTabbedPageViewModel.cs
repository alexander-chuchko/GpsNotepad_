using Acr.UserDialogs;
using GpsNotepad.Extension;
using GpsNotepad.Helpers;
using GpsNotepad.Model;
using GpsNotepad.Model.Pin;
using GpsNotepad.Recource;
using GpsNotepad.Service.Authorization;
using GpsNotepad.Services.ImagesOfPin;
using GpsNotepad.Services.Pin;
using GpsNotepad.View;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class MainListTabbedPageViewModel: BaseViewModel, IInitialize, INavigatedAware
    {
        #region---PrivateFields---

        private readonly IPinServices _pinServices;
        private readonly IAuthorizationService _authorizationService;
        private readonly IImagesPinService _imagesPinService;
        private ObservableCollection<PinViewModel> _firstValueOfList = null;

        #endregion

        public MainListTabbedPageViewModel(INavigationService navigationService,
            IPinServices pinServices,
            IAuthorizationService authorizationService,
            IImagesPinService imagesPinService) : base(navigationService)
        {
            _authorizationService = authorizationService;
            _pinServices = pinServices;
            _imagesPinService = imagesPinService;
            PinViewModelList = new ObservableCollection<PinViewModel>();
        }

        #region  ---  PublicProperties  ---

        private bool _ExitSearch;
        public bool ExitSearch
        {
            get => _ExitSearch;
            set => SetProperty(ref _ExitSearch, value);
        }

        private ObservableCollection<PinViewModel> _PlacesList;
        public ObservableCollection<PinViewModel> PlacesList
        {
            get { return _PlacesList; }
            set { SetProperty(ref _PlacesList, value); }
        }

        private bool _IsVisibleCommand;
        public bool IsVisibleCommand
        {
            get => _IsVisibleCommand;
            set => SetProperty(ref _IsVisibleCommand, value);
        }

        private int _SizeRow =ListOfConstants.HeightRow;
        public int SizeRow
        {
            get => _SizeRow;
            set => SetProperty(ref _SizeRow, value);
        }

        private int _SizeHightListView;

        public int SizeHightListView
        {
            get => _SizeHightListView;
            set => SetProperty(ref _SizeHightListView, value);
        }

        private string _SearchText;
        public string SearchText
        {
            get => _SearchText;
            set => SetProperty(ref _SearchText, value);
        }

        private string _ImageSource = "ic_like_gray.png";
        public string ImageSource
        {
            get { return _ImageSource; }
            set { SetProperty(ref _ImageSource, value); }
        }

        private PinViewModel _PinViewModel;
        public PinViewModel PinViewModel
        {
            set { SetProperty(ref _PinViewModel, value); }
            get { return _PinViewModel; }
        }

        private bool _IsVisibleListView;
        public bool IsVisableListView
        {
            get { return _IsVisibleListView; }
            set { SetProperty(ref _IsVisibleListView, value); }
        }

        private bool _IsVisibleLabel;
        public bool IsVisableLabel
        {
            get { return _IsVisibleLabel; }
            set { SetProperty(ref _IsVisibleLabel, value); }
        }

        private ObservableCollection<PinViewModel> _PinViewModelList;
        public ObservableCollection<PinViewModel>  PinViewModelList
        {
            get { return _PinViewModelList; }
            set { SetProperty(ref _PinViewModelList, value); }
        }

        private ICommand _NavigationToSettingsCommand;
        public ICommand NavigationToSettingsCommand => _NavigationToSettingsCommand ?? new Command(OnNavigationToSettings);


        private ICommand _NavigationToMainPageCommand;
        public ICommand NavigationToMainPageCommand => _NavigationToMainPageCommand ?? new Command(OnNavigationToMainPage);


        private ICommand _BackTapCommand;
        public ICommand BackTapCommand => _BackTapCommand ?? (_BackTapCommand = new Command(OnBackTap));


        private ICommand _RemoveCommand;
        public ICommand RemoveCommand => _RemoveCommand ?? new Command(OnRemoveModel);


        private ICommand _UpdateCommand;
        public ICommand UpdateCommand => _UpdateCommand ?? new Command(OnUpdateModel);


        private ICommand _ImageTapCommand;
        public ICommand ImageTapCommand => _ImageTapCommand ?? new Command(OnImageTap);


        private ICommand _ItemTappedCommand;
        public ICommand ItemTappedCommand => _ItemTappedCommand ?? new Command(OnItemTapped);


        private ICommand _NavigationToAddPinCommand;
        public ICommand NavigationToAddPinCommand => _NavigationToAddPinCommand ?? new Command(OnNavigationToAddPin);
        #endregion

        #region     ---    Methods   ---

        private async void OnNavigationToMainPage()
        {
            LoggingOutUser();
            await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainPage)}");
        }
        private void OnImageTap(object parametr)
        {
            PinViewModel pinViewModel = parametr as PinViewModel; //Corrrect property PinViewModel 
            if (pinViewModel != null)
            {
                pinViewModel.Favorite = pinViewModel.Favorite == false ? true : false;
                pinViewModel.ImagePath= pinViewModel.Favorite == false ? "ic_like_gray.png" : "ic_like_blue.png";
            }
        }
        private void OnBackTap()
        {
            ExitSearch = true;
        }

        private async void OnNavigationToAddPin()
        {
            await _navigationService.NavigateAsync($"{ nameof(AddEditPinView)}");
        }

        private async void OnNavigationToSettings()
        {
            await _navigationService.NavigateAsync($"{ nameof(SettingsView)}");

        }

        private void OnItemTapped(object parametr)
        {
            PinViewModel pinViewModel = parametr as PinViewModel;
            if (pinViewModel != null)
            {
                PinViewModel = pinViewModel;
                ShowPinOnMap();
            }
        }

        private async void OnUpdateModel(object selectObject)
        {
            PinViewModel  pinViewModel = selectObject as PinViewModel;

            if (pinViewModel != null)
            {
                var parametr = new NavigationParameters();
                parametr.Add(ListOfConstants.PinModel, pinViewModel);
                await _navigationService.NavigateAsync($"{ nameof(AddEditPinView)}", parametr);
            }
        }

        private void LoggingOutUser() //When logging out, delete all user settings
        {
            _authorizationService.Unauthorize();
        }

        private async void OnExecuteGoToMainMap()
        {
            await _navigationService.NavigateAsync($"{ nameof(MainMapTabbedPageView)}");
        }

        public IEnumerable<PinViewModel> ConvertingPinModelToPinViewModel(IEnumerable<PinModel> PinModellist)
        {
            var pinViewModelList = new ObservableCollection<PinViewModel>();
            foreach (var pinModel in PinModellist)
            {
                var convertingPinViewModel = pinModel.ToPinViewModel();
                if (convertingPinViewModel != null)
                {
                    convertingPinViewModel.ImagePath = convertingPinViewModel.Favorite == false ? "ic_like_gray.png" : "ic_like_blue.png";
                    pinViewModelList.Add(convertingPinViewModel);
                }
            }
            return pinViewModelList;
        }

        private async void OnRemoveModel(object selectObject)
        {
            PinViewModel  pinViewModel = selectObject as PinViewModel;
            if (pinViewModel != null)
            {
                var confirmConfig = new ConfirmConfig()
                {
                    Message = AppResource.you_really_want_to_delete_this_profile,
                    OkText = AppResource.delete.ToUpper(),
                    CancelText = AppResource.cancel.ToUpper()
                };

                var result = await UserDialogs.Instance.ConfirmAsync(confirmConfig);
                
                if (result)
                {
                    var pinModel = pinViewModel.ToPinModel();

                    bool resultDeleteOfImagesPin = await _imagesPinService.DeleteAllImagePinModelAsync(pinModel.Id);
                    bool resultDeleteOfPinModel = await _pinServices.DeletePinModelToStorageAsync(pinModel);
                    
                    if (resultDeleteOfPinModel&& resultDeleteOfImagesPin)
                    {
                        PinViewModelList.Remove(pinViewModel);
                    }
                    if (PinViewModelList.Count == 0)
                    {
                        ToggleVisibility(true, false);
                    }
                }
            }
        }

        private void ToggleVisibility(bool visableLabel, bool visableListView)
        {
            IsVisableListView = visableListView;
            IsVisableLabel = visableLabel;
        }

        private async void ShowPinOnMap()
        {
            var parametr = new NavigationParameters();
            parametr.Add(ListOfConstants.SelectedPin, PinViewModel);
            await _navigationService.SelectTabAsync($"{ nameof(MainMapTabbedPageView)}", parametr);
        }

        private async void ShowRelevantPins() //Refactor
        {
            if(PinViewModelList.Count!=0)
            {
                if (_firstValueOfList == null)
                {
                    _firstValueOfList = PinViewModelList;
                }
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    var resultGetPins = await _pinServices.GetPinListAsync(SearchText);
                    if (resultGetPins.Count != 0 && resultGetPins != null)
                    {
                        var profileViewModelList = ConvertingPinModelToPinViewModel(resultGetPins);
                        PinViewModelList = (ObservableCollection<PinViewModel>)profileViewModelList;
                        ToggleVisibility(false, true);
                    }
                    else
                    {
                        ToggleVisibility(true, false);//Correct
                    }
                }
                else
                {
                    ToggleVisibility(false, true);
                    PinViewModelList = _firstValueOfList;
                }
            }
        }

        public async void Initialize(INavigationParameters parameters)
        {
            var listPinModelsOfCurrentUser = await _pinServices.GetPinListAsync();
            if (listPinModelsOfCurrentUser.ToList().Count == 0 || listPinModelsOfCurrentUser == null)
            {
                ToggleVisibility(true, false);
            }
            else
            {
                ToggleVisibility(false, true);
                var profileViewModelList = ConvertingPinModelToPinViewModel(listPinModelsOfCurrentUser);
                PinViewModelList.AddRange(profileViewModelList);
            }
        }

        #endregion

        #region    ---   Overriding   ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(SearchText))
            {
                ShowRelevantPins();
            }
        }
        #endregion

        #region      ---  Iterface INavigatedAware implementation   ---

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinViewModel>(ListOfConstants.PinViewModel, out PinViewModel pinViewModel))
            {
                var pinViewModelUpdate = PinViewModelList.FirstOrDefault(x => x.Id == pinViewModel.Id);
                if (pinViewModelUpdate != null)
                {
                    int index = PinViewModelList.IndexOf(pinViewModelUpdate);
                    PinViewModelList[index].Label = pinViewModel.Label;
                    PinViewModelList[index].Description = pinViewModel.Description;
                    PinViewModelList[index].Latitude = pinViewModel.Latitude;
                    PinViewModelList[index].Longitude = pinViewModel.Longitude;
                    PinViewModelList[index].Address = pinViewModel.Address;
                }
                else
                {
                    PinViewModelList.Add(pinViewModel);
                    if (PinViewModelList.Count != 0 && !IsVisableListView)
                    {
                        ToggleVisibility(false, true);
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
