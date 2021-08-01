using Acr.UserDialogs;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GpsNotepad.Services.Media
{
    public class MediaService: IMediaService
    {
        public async Task<string> GetPhotoFromGalleryAsync()
        {
            string selectedImage = null;
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                selectedImage = photo.FullPath;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
            return selectedImage;
        }

        public async Task<string> TakingPicturesAsync()
        {
            string takingPicturesImage = null;
            try
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null)
                {
                    var newFile = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                    using (var stream = await result.OpenReadAsync())
                    using (var newStream = File.OpenWrite(newFile))
                        await stream.CopyToAsync(newStream);
                    takingPicturesImage = result.FullPath;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
            return takingPicturesImage;
        }
    }
}
