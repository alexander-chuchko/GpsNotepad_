using GpsNotepad.Model;
using GpsNotepad.Model.Pin;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Extension
{
    public static class PinExtension
    {
        public static PinModel ToPinModel(this PinViewModel pinViewModel)
        {
            PinModel  pinModel = null;
            if (pinViewModel!=null)
            {
                pinModel = new PinModel
                {
                    Id= pinViewModel.Id,
                    Description=pinViewModel.Description,
                    UserId=pinViewModel.UserId,
                    Address=pinViewModel.Address,
                    Label=pinViewModel.Label,
                    Latitude=pinViewModel.Latitude,
                    Longitude=pinViewModel.Longitude
                };
            }
            return pinModel;
        }
        public static PinViewModel ToPinViewModel(this PinModel pinModel)
        {
            PinViewModel  pinViewModel = null;
            if(pinModel != null)
            {
                pinViewModel = new PinViewModel
                {
                    Id=pinModel.Id,
                    UserId = pinModel.UserId,
                    Latitude=pinModel.Latitude,
                    Longitude=pinModel.Longitude,
                    Label=pinModel.Label,
                    Address=pinModel.Address,
                    Description=pinModel.Description
                };
            }
            return pinViewModel;
        }
        public static Pin ToPin(this PinModel pinModel)
        {
            Pin pin = null;
            if (pinModel != null)
            {
                pin = new Pin
                {
                    Label = pinModel.Label,
                    Position = new Position(pinModel.Latitude, pinModel.Longitude),
                    Address = pinModel.Address
                };
            }
            return pin;
        }
    }
}
