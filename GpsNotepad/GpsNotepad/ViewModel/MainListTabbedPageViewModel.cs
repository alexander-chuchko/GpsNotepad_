﻿using Acr.UserDialogs;
using GpsNotepad.Extension;
using GpsNotepad.Helpers;
using GpsNotepad.Model;
using GpsNotepad.Model.Pin;
using GpsNotepad.Recource;
using GpsNotepad.Service.Authorization;
using GpsNotepad.Services.Pin;
using GpsNotepad.View;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class MainListTabbedPageViewModel: BaseViewModel, IInitializeAsync
    {
        #region---PrivateFields---
        private bool _isVisibleLabel;
        private bool _isVisibleListView;
        
        private PinViewModel _pinViewModel;
        private ObservableCollection<PinViewModel> _pinViewModelList;
        private readonly IPinServices _pinServices;
        private readonly IAuthorizationService _authorizationService;

        #endregion
        public MainListTabbedPageViewModel(INavigationService navigationService, IPinServices pinServices, IAuthorizationService authorizationService) : base(navigationService)
        {

            _authorizationService = authorizationService;
            _pinServices = pinServices;
           NavigationToSettingsView = new Command(ExecuteGoToSettingsPage);
           NavigationToAddPin = new Command(ExecuteGoToAddPin);
           RemoveCommand = new Command(RemoveModel);
           UpdateCommand = new Command(UpdateModel);
           NavigationToSingIn = new Command(ExecuteGoToSignIn);
           NavigationToMainMap = new Command(ExecuteGoToMainMap);
           PinViewModelList = new ObservableCollection<PinViewModel>();
            //PathPicture = "ic_like_gray.png";
        }

        #region---PublicProperties---
        public ICommand NavigationToSettingsView { get; set; }
        public ICommand NavigationToAddPin { get; set; }
        public ICommand NavigationToSingIn { get; set; }
        public ICommand NavigationToMainMap { get; set; }
        public ICommand RemoveCommand { set; get; }
        public ICommand UpdateCommand { set; get; }


        public PinViewModel PinViewModel
        {
            set { SetProperty(ref _pinViewModel, value); }
            get { return _pinViewModel; }
        }
        public bool IsVisableListView
        {
            get { return _isVisibleListView; }
            set { SetProperty(ref _isVisibleListView, value); }
        }
        public bool IsVisableLabel
        {
            get { return _isVisibleLabel; }
            set { SetProperty(ref _isVisibleLabel, value); }
        }
        public ObservableCollection<PinViewModel>  PinViewModelList
        {
            get { return _pinViewModelList; }
            set { SetProperty(ref _pinViewModelList, value); }
        }
        #endregion

        #region---Methods---
        private async void ExecuteGoToAddPin()
        {
            await _navigationService.NavigateAsync($"{ nameof(AddEditPinView)}");
        }
        private async void ExecuteGoToSettingsPage()
        {
            await _navigationService.NavigateAsync($"{ nameof(SettingsView)}");
        }
        private async void UpdateModel(object selectObject)
        {
            PinViewModel  pinViewModel = selectObject as PinViewModel;
            if (pinViewModel != null)
            {
                var parametr = new NavigationParameters();
                parametr.Add(ListOfNames.pinModel, pinViewModel);
                await _navigationService.NavigateAsync($"{ nameof(AddEditPinView)}", parametr);
            }
        }
        private void DeletingCurrentUserSettings() //When logging out, delete all user settings
        {
            _authorizationService.Unauthorize();
        }
        private async void ExecuteGoToMainMap()
        {
            //await _navigationService.NavigateAsync($"{ nameof(NavigationPage)}/{ nameof( MainMapTabbedPageView)}");
            await _navigationService.NavigateAsync($"{ nameof(MainMapTabbedPageView)}");
        }

        private async void ExecuteGoToSignIn()
        {
            DeletingCurrentUserSettings();
            await _navigationService.NavigateAsync($"{ nameof(NavigationPage)}/{ nameof(SignInView)}");
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
        private async void RemoveModel(object selectObject)
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
                    bool resultOfAction = await _pinServices.DeletePinModelToStorageAsync(pinModel);
                    if (resultOfAction)
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
            parametr.Add(ListOfNames.selectedPin, PinViewModel);
            //await _navigationService.GoBackAsync(parametr);
            //NavigationPage page = (NavigationPage)App.Current.MainPage;
           //var count= page.Navigation.NavigationStack.Count;
            await _navigationService.SelectTabAsync($"{ nameof(MainMapTabbedPageView)}", parametr);
        }
        #endregion
        #region---Overriding---

        public async Task InitializeAsync(INavigationParameters parameters)
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
        /*
         //Try to run IOS
        public void Initialize(INavigationParameters parameters)
        {
            GetAllPins();
            //Task.Run(() => GetAllPins());
            if (_pins.ToList().Count == 0 || _pins == null)
            {
                ToggleVisibility(true, false);
            }
            else
            {
                ToggleVisibility(false, true);
                var profileViewModelList = ConvertingPinModelToPinViewModel(_pins);
                PinViewModelList.AddRange(profileViewModelList);
            }
        }

        private async void GetAllPins()
        {
            IEnumerable <PinModel> pins = new List<PinModel>();
            pins = await _pinServices.GetPinListAsync();
            _pins = pins;
        }
        */
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(PinViewModel))
            {
                ShowPinOnMap();
            }
        }
        #endregion

    }
}
