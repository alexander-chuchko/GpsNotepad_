using GpsNotepad.Model;
using GpsNotepad.Model.ImagePin;
using System.IO;

namespace GpsNotepad.Extension
{
    public static class ImagePinExtension
    {
        public static ImagesPin ToImagesPin(this ImagePinViewModel imagePinViewModel)
        {
            ImagesPin imagesPin = null;
            if (imagePinViewModel != null)
            {
                imagesPin = new ImagesPin
                {
                    Id = imagePinViewModel.Id,
                    PinId=imagePinViewModel.PinId,
                    PathImage=imagePinViewModel.PathImage
                };
            }
            return imagesPin;
        }

        public static ImagePinViewModel ToImagePinViewModel(this ImagesPin imagesPin)
        {
            ImagePinViewModel  imagePinViewModel = null;
            if (imagesPin != null)
            {
                imagePinViewModel = new ImagePinViewModel
                {
                    Id=imagesPin.Id,
                    PinId=imagesPin.PinId,
                    PathImage=imagesPin.PathImage,
                    NameImage=Path.GetFileName(imagesPin.PathImage) 
                };
            }
            return imagePinViewModel;
        }
    }
}
