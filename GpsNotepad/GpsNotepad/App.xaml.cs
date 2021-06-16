using GpsNotepad.Service;
using GpsNotepad.Service.Authorization;
using GpsNotepad.Service.Settings;
using GpsNotepad.Service.User;
using GpsNotepad.Services.Camera;
using GpsNotepad.Services.Pin;
using GpsNotepad.Services.Repository;
using GpsNotepad.View;
using GpsNotepad.ViewModel;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;

namespace GpsNotepad
{
    public partial class App : PrismApplication
    {
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
            //Services___!
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            
            containerRegistry.RegisterInstance<IUserService>(Container.Resolve<UserService>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<ICameraService>(Container.Resolve<CameraService>());
            containerRegistry.RegisterInstance<IPinServices>(Container.Resolve<PinServices>());

            //Registration
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInView, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpView, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<MainMapTabbedPageView, MainMapTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<MainListTabbedPageView, MainListTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPinView, AddEditPinViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
        }
        protected override async void OnInitialized()
        {
            InitializeComponent();
            var result= await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(SignInView)}");
            //var result = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainMapTabbedPageView)}");
            //var result= await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainListTabbedPageView)}");
            //var result = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(AddEditPinView)}");
            if (!result.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
        }
        #endregion
    }
}
