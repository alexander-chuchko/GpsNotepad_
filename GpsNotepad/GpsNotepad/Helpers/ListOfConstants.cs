namespace GpsNotepad.Helpers
{
    public class ListOfConstants
    {
        //the keys
        public const string NewUser = "NewUser";
        public const string UserRegistrationData = "UserRegistrationData";
        public const string PinModel = "PinModel";
        public const string PinViewModel = "PinViewModel";
        public const string SelectedPin = "SelectedPin";
        public const string PathSelectedPicture = "PathPicture";
        public const string SelectedPinData = "PinData";
        public const string SelectedImage = "SelectedImage";
        //languages
        public const string English = "English";

        //pictures
        public const string BasePicture = "pic_Pin.png";
        public const string PictureForFolder = "folder_image.png";
        public const string PictureForCamera = "camera.png";

        //button picture
        public const string ButtonClear = "ic_clear.png";
        public const string ButtonEye = "ic_eye.png";
        public const string ButtonEyeOff = "ic_eye_off.png";


        //placeholder
        public const string PlaceholderEnterEmail = "Enter email";
        public const string PlaceholderEnterPassword = "Enter password";
        public const string PlaceholderEnterName = "Enter you name";
        public const string PlaceholderEnterConfirmPassword = "Repeat password";
        public const string PlaceholderLabel = "Enter the label";
        public const string PlaceholderDescription = "Write a description";
        public const string PlaceholderLongitude = "Longitude";
        public const string PlaceholderLatitude = "Latitude";

        //error
        public const string WrongEmail = "Wrong Email";
        public const string WrongPassword = "The password is incorrect";
        public const string WrongName = "The name is incorrect";
        public const string WrongConfirmPassword = "Password mismatch";
        public const string WrongLabel = "Invalid label";
        public const string WrongDescription = "Invalid description";
        public const string WrongLatitude = "Wrong latitude";
        public const string WrongLongitude = "Wrong longitude";


        //size
        public const int NumberOfDisplayedPictures = 3;
        public const int ItemSpacingViewCollection = 4;
        public const int ItemVisibleElementOfListView = 5;
        public const int NumberOfVisibleListViewItemsForPageAddPin = 2;
        public const int SizeRow = 50;
        public const int HeightRow = 50;
        public const int HeightRowForAddPage = 30;

        //valid coordinates for input
        public const int MinLatitude = -90;
        public const int MaxLatitude = 90;

        public const int MinLongitude = -180;
        public const int MaxLongitude = 180;

    }
}
