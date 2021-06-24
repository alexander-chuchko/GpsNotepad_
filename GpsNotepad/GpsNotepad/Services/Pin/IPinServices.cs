using GpsNotepad.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GpsNotepad.Services.Pin
{
    public interface IPinServices
    {
        Task<List<PinModel>> GetPinListAsync(string keyWord= null);
        Task<bool> SaveOrUpdatePinModelToStorageAsync(PinModel pinModel);
        Task<bool> DeletePinModelToStorageAsync(PinModel pinModel);
        Task<bool> UpdatePinModelToStorageAsync(PinModel pinModel);
        void SetStateOfTextInSearchBar(string value);
        string GetStatusPermission();
    }
}
