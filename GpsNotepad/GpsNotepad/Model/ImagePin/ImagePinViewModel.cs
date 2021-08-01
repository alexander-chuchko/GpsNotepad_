using Prism.Mvvm;

namespace GpsNotepad.Model.ImagePin
{
    public class ImagePinViewModel: BindableBase
    {

        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }

        private int _PinId;
        public int PinId
        {
            get { return _PinId; }
            set { SetProperty(ref _PinId, value); }
        }
        private string _PathImage;
        public string PathImage
        {
            get { return _PathImage; }
            set { SetProperty(ref _PathImage, value); }
        }

        private string _NameImage;
        public string NameImage
        {
            get { return _NameImage; }
            set { SetProperty(ref _NameImage, value); }
        }
    }
}
