using GpsNotepad.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GpsNotepad.Services.ImagesOfPin
{
    public interface IImagesPinService
    {
        Task<IEnumerable<ImagesPin>> GetAllImagePinModelAsync(int pinId);
        Task<bool> DeleteImagePinModelAsync(ImagesPin imagePinModel);
        Task<bool> DeleteAllImagePinModelAsync(int pinId);
        Task<bool> SaveImagePinModelAsync(ImagesPin imagePinModel);
        Task<bool> UpdateImagePinModelAsync(ImagesPin imagePinModel);
    }
}
