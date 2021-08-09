using Acr.UserDialogs;
using GpsNotepad.Enum;
using GpsNotepad.Helpers;
using GpsNotepad.Popup;
using GpsNotepad.Service;
using GpsNotepad.Service.Authorization;
using GpsNotepad.Service.Settings;
using GpsNotepad.Service.User;
using GpsNotepad.Services.Camera;
using GpsNotepad.Services.ImagesOfPin;
using GpsNotepad.Services.Media;
using GpsNotepad.Services.Permissions;
using GpsNotepad.Services.Pin;
using GpsNotepad.Services.Repository;
using GpsNotepad.Services.Theme;
using GpsNotepad.Services.TimeZone;
using GpsNotepad.View;
using GpsNotepad.ViewModel;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GpsNotepad
{
    public partial class App : PrismApplication
    {
        private IAuthorizationService _AuthorizationService;
        IAuthorizationService AuthorizationService => _AuthorizationService ?? (_AuthorizationService = Container.Resolve<IAuthorizationService>());
        
        private IThemeService _ThemeService;
        IThemeService ThemeService => _ThemeService ?? (_ThemeService = Container.Resolve<IThemeService>());

        public App() : this(null)
        {

        }
        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        #region---Overrides---
        protected override void OnStart()
        {
        }
        protected override void OnSleep()
        {
        }
        protected override void OnResume()
        {
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Registering services
            containerRegistry.RegisterInstance(UserDialogs.Instance);
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IUserService>(Container.Resolve<UserService>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<ICameraService>(Container.Resolve<CameraService>());
            containerRegistry.RegisterInstance<IPinServices>(Container.Resolve<PinServices>());
            containerRegistry.RegisterInstance<IPermissionService>(Container.Resolve<PermissionService>());
            containerRegistry.RegisterInstance<IImagesPinService>(Container.Resolve<ImagesPinService>());
            containerRegistry.RegisterInstance<IMediaService>(Container.Resolve<MediaService>());
            containerRegistry.RegisterInstance<IThemeService>(Container.Resolve<ThemeService>());
            containerRegistry.RegisterInstance<ITimeZoneService>(Container.Resolve<TimeZoneService>());

            //Registering pages
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInView, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainMapTabbedPageView, MainMapTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<MainListTabbedPageView, MainListTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPinView, AddEditPinViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<TabbedPage1>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView2, SignUpViewModel2>();
            containerRegistry.RegisterForNavigation<ColorClockView, ColorClockViewModel>();

            //Modal pages
            containerRegistry.RegisterForNavigation<PopupView, PopupViewModel>();
            containerRegistry.RegisterForNavigation<PhotoView ,PhotoViewModel>();
            containerRegistry.RegisterForNavigation<ClockView, ClockViewModel>();

        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            
            var displayInfo = DeviceDisplay.MainDisplayInfo;

            double displayWidth = displayInfo.Width;

            var widthImageForCollectionView = ((displayInfo.Width/ displayInfo.Density) - ListOfConstants.ItemSpacingViewCollection) /
                ListOfConstants.NumberOfDisplayedPictures;

            Resources.Add(nameof(widthImageForCollectionView), widthImageForCollectionView);
    
            //var result1 = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}");
            /*
            var result1 = await NavigationService.NavigateAsync(nameof(SignUpView2));
            if (!result1.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
            */

            if (AuthorizationService.IsAuthorized)
            {
                if (ThemeService.GetValueTheme() != EnumSet.Theme.Light)
                {
                    ThemeService.PerformThemeChange(ThemeService.GetValueTheme());
                }
                //var result = await NavigationService.NavigateAsync($"{nameof(SettingsView)}");
                //var result = await NavigationService.NavigateAsync($"{ nameof(ClockView)}");
                //var result = await NavigationService.NavigateAsync($"{nameof(AddEditPinView)}");
                //var result= await NavigationService.NavigateAsync($"/{ nameof(NavigationPage)}/{ nameof(TabbedPage1)}");
                var result =await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(TabbedPage1)}");
                //var result = await NavigationService.NavigateAsync($"{nameof(MainPage)}");
                if (!result.Success)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
            else
            {
                //var result = await NavigationService.NavigateAsync($"{nameof(SettingsView)}");
                //var result = await NavigationService.NavigateAsync($"{nameof(AddEditPinView)}");
                //var result = await NavigationService.NavigateAsync($"{ nameof(TabbedPage1)}");
                //var result=  await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(SignInView)}");
                //var result = await NavigationService.NavigateAsync($"{ nameof(ClockView)}");
                var result = await NavigationService.NavigateAsync($"/{ nameof(NavigationPage)}/{ nameof(MainPage)}");
                if (!result.Success)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }
        #endregion
    }
}
