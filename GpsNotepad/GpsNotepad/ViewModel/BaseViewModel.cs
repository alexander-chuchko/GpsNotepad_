using Prism.Mvvm;
using Prism.Navigation;

namespace GpsNotepad.ViewModel
{
    public class BaseViewModel : BindableBase
    {
        #region   ---    PrivateFields   ---

        protected readonly INavigationService _navigationService;

        #endregion

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }  
    }
}