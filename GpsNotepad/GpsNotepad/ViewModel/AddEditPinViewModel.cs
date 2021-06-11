using GpsNotepad.Model.Pin;
using GpsNotepad.Service.Authorization;
using Prism.Navigation;
using GpsNotepad.Services.Pin;
using Xamarin.Forms;
using System.Windows.Input;
using GpsNotepad.Helpers;
using GpsNotepad.Recource;
using GpsNotepad.View;
using Prism.Services;
using System.Threading.Tasks;
using GpsNotepad.Extension;
using System;

namespace GpsNotepad.ViewModel
{
    public class AddEditPinViewModel:BaseViewModel, INavigatedAware
    {
        #region---PrivateFields---
        private string _label;
        private string _description;
        private string _latitude;
        private string _longitude;
        private bool _isEnable;
        private PinViewModel  pinViewModel;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPinServices _pinServices;
        private readonly IPageDialogService _pageDialogService;
        #endregion
        public AddEditPinViewModel(INavigationService navigationService, IAuthorizationService authorizationService, IPinServices pinServices, IPageDialogService pageDialogService) :base(navigationService)
        {
            Label = string.Empty;
            Description = string.Empty;
            Longitude = string.Empty;
            Latitude = string.Empty;
            IsEnable = false;
            _authorizationService = authorizationService;
            _pinServices = pinServices;
            _pageDialogService = pageDialogService;
            SaveCommand = new Command(SaveOrUpdatePinModel);
        }
        #region---PublicProperties---

        public ICommand SaveCommand { get; set; }
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
            set { SetProperty(ref _label, value); }
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
        #endregion
        #region---Methods---
        private async void ExecuteNavigateToNavigationToMainList()
        {
            //await _navigationService.GoBackAsync();
            await _navigationService.NavigateAsync(($"/{ nameof(NavigationPage)}/{ nameof(MainListTabbedPageView)}"));
        }
        private bool IsFieldsFilled()
        {
            bool resultFilling = true;
            if (!Validation.IsInformationInNameAndNickName(Label, Latitude, Longitude))
            {
                resultFilling = false;
            }
            return resultFilling;
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
                    ExecuteNavigateToNavigationToMainList();
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
            PinViewModel.Latitude =Convert.ToDouble(Latitude);
            PinViewModel.Longitude =Convert.ToDouble(Longitude);
            var pinModel = PinViewModel.ToPinModel();
            if (pinModel != null)
            {
                resultOfAction = await _pinServices.SaveOrUpdatePinModelToStorageAsync(pinModel);
            }
            return resultOfAction;
        }
        private async Task<bool> AddPinModel()
        {
            bool resultOfAction = false;
            PinViewModel PinViewModel = new PinViewModel()
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
            }
            return resultOfAction;
        }

        #endregion

        #region--Iterface INavigatedAware implementation-- 
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinViewModel>(ListOfNames.pinModel, out PinViewModel pinViewModel))
            {
                PinViewModel = parameters.GetValue<PinViewModel>(ListOfNames.pinModel);
                if (PinViewModel != null)
                {
                    Label = PinViewModel.Label;
                    Description = PinViewModel.Description;
                    Longitude = PinViewModel.Longitude.ToString(); //No forget
                    Latitude = PinViewModel.Latitude.ToString(); 
                }
            }
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }
        #endregion

    }
}
