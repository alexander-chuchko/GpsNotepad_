using Acr.UserDialogs;
using GpsNotepad.Service;
using GpsNotepad.Service.Authorization;
using GpsNotepad.Service.Settings;
using GpsNotepad.Service.User;
using GpsNotepad.Services.Camera;
using GpsNotepad.Services.Permissions;
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
        private IAuthorizationService _authorizationService;
        IAuthorizationService AuthorizationService => _authorizationService ?? (_authorizationService = Container.Resolve<IAuthorizationService>());
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
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            //var result1 = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}");
            
            var result1 = await NavigationService.NavigateAsync(nameof(SignUpView2));
            if (!result1.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
            
            /*
            if (AuthorizationService.IsAuthorized)
            {
                //var result= await NavigationService.NavigateAsync($"/{ nameof(NavigationPage)}/{ nameof(TabbedPage1)}");
                var result=await NavigationService.NavigateAsync($"{ nameof(TabbedPage1)}");
                if (!result.Success)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
            else
            {
                //var result=  await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(SignInView)}");
                var result = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}");
                if (!result.Success)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
           */
            //var result= await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(SignInView)}");
            //var result = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainMapTabbedPageView)}");
            //var result= await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainListTabbedPageView)}");
            //var result = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(AddEditPinView)}");
        }
        #endregion
    }
}
