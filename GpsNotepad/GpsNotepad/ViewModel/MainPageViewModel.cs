using GpsNotepad.View;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(INavigationService navigationService) :base(navigationService)
        {
            
        }
        private ICommand _NavigationToSignInCommand;
        public ICommand NavigationToSignInCommand => _NavigationToSignInCommand ?? (_NavigationToSignInCommand= new Command(OnNavigationToSignIn));

        private ICommand _NavigationSignUpCommand;
        public ICommand NavigationSignUpCommand => _NavigationSignUpCommand ?? (_NavigationSignUpCommand= new Command(OnNavigationSignUp));

        private async void OnNavigationToSignIn()
        {
            await _navigationService.NavigateAsync(nameof(SignInView));
        }
        private async void OnNavigationSignUp()
        {
            await _navigationService.NavigateAsync(nameof(SignUpView));
        }
    }
}
