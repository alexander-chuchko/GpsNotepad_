using GpsNotepad.Helpers;
using GpsNotepad.Model.ImagePin;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GpsNotepad.ViewModel
{
    public class ImagesPins
    {
        public int id { get; set; }
        public string PathImage { get; set; }
        public int UserId { get; set; }
    }
    public class PhotoViewModel : BaseViewModel, INavigatedAware
    {
        public PhotoViewModel(INavigationService navigationService) : base(navigationService)
        {

            //PathPicture = "pic_profile3.png";
        }

        private List<ImagesPins> _ImagesPins = new List<ImagesPins>()
        {
            new ImagesPins { id = 1, PathImage = "pic_enter_page.png", UserId = 0 },
            new ImagesPins { id = 2, PathImage = "pic_enter_page.png", UserId = 0 },
            new ImagesPins { id = 3, PathImage = "pic_enter_page.png", UserId = 0 },
            new ImagesPins { id = 4, PathImage = "pic_profile3.png", UserId = 0 }
        };

        public List<ImagesPins> ImagesPins1
        {
            get { return _ImagesPins; }
            set { SetProperty(ref _ImagesPins, value); }
        }


        private List<string> _ListPathPicture = new List<string> { "pic_enter_page.png",
            "pic_profile1.png ",
            "pic_profile2.png",
            "pic_profile3.png" };
        public List<string> ListPathPicture
        {
            get { return _ListPathPicture; }
            set { SetProperty(ref _ListPathPicture, value); }
        }


        #region---PublicProperties---

        private int _CurrentIndex;
        public int CurrentIndex
        {
            get { return _CurrentIndex; }
            set { SetProperty(ref _CurrentIndex, value); }
        }

        private int _TotalItems;
        public int TotalItems
        {
            get { return _TotalItems; }
            set { SetProperty(ref _TotalItems, value); }
        }

        private string _CurrentIndexToDisplay;
        public string CurrentIndexToDisplay
        {
            get { return _CurrentIndexToDisplay; }
            set { SetProperty(ref _CurrentIndexToDisplay, value); }
        }

        private string _TotalItemsToDisplay;
        public string TotalItemsToDisplay
        {
            get { return _TotalItemsToDisplay; }
            set { SetProperty(ref _TotalItemsToDisplay, value); }
        }

        private ImagePinViewModel _ImagePinViewModel;
        public ImagePinViewModel ImagePinViewModel_
        {
            get { return _ImagePinViewModel; }
            set { SetProperty(ref _ImagePinViewModel, value); }
        }

        private ObservableCollection<ImagePinViewModel> _ImagePinViewModels;

        public ObservableCollection<ImagePinViewModel> ImagePinViewModels
        {
            get { return _ImagePinViewModels; }
            set { SetProperty(ref _ImagePinViewModels, value); }
        }

        private string _PathPicture;
        public string PathPicture
        {
            get { return _PathPicture; }
            set { SetProperty(ref _PathPicture, value); }
        }

        private ICommand _NavigationToMainMapCommand;
        public ICommand NavigationToMainMapCommand => _NavigationToMainMapCommand ?? new Command(OnNavigationToMainMap);


        private ICommand _SwipeLeftCommand;
        public ICommand SwipeLeftAndRightCommand => _SwipeLeftCommand ?? new Command(OnSwipeLeftAndRight);


        private void OnSwipeLeftAndRight(object parametr)
        {

            if(parametr.ToString()== "Right" && CurrentIndex > 0)
            { 
                CurrentIndex--;
                PathPicture = ImagePinViewModels[CurrentIndex].PathImage;
                CurrentIndexToDisplay = (CurrentIndex + 1).ToString();
            }
            else if(parametr.ToString()== "Left" && CurrentIndex < TotalItems-1)
            {
                CurrentIndex++;
                PathPicture = ImagePinViewModels[CurrentIndex].PathImage;
                CurrentIndexToDisplay = (CurrentIndex + 1).ToString();
            }
        }

        #endregion

        private void OnNavigationToMainMap()
        {
            _navigationService.GoBackAsync();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<(ImagePinViewModel, ObservableCollection<ImagePinViewModel>)>(ListOfConstants.SelectedImage, out (ImagePinViewModel, ObservableCollection<ImagePinViewModel>) imagePinData))
            {
                ImagePinViewModel_ = imagePinData.Item1;
                ImagePinViewModels = imagePinData.Item2;
                
                if(ImagePinViewModel_!=null)
                {
                    PathPicture = ImagePinViewModel_.PathImage;
                }
                TotalItems = ImagePinViewModels.Count;
                TotalItemsToDisplay = TotalItems.ToString();
                CurrentIndex = ImagePinViewModels.IndexOf(ImagePinViewModel_);
                CurrentIndexToDisplay=(CurrentIndex + 1).ToString();
            }
        }
    }
}

