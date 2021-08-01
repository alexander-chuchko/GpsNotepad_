using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Media
{
    public interface IMediaService
    {
        Task<string> GetPhotoFromGalleryAsync();
        Task<string> TakingPicturesAsync();
    }
}
