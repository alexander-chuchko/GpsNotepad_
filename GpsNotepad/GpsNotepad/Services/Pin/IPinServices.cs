using GpsNotepad.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using GpsNotepad.Model.Pin;

namespace GpsNotepad.Services.Pin
{
    public interface IPinServices
    {
        Task<List<PinModel>> GetPinListAsync();
        Task<bool> SaveOrUpdatePinModelToStorageAsync(PinModel pinModel);
        Task<bool> DeletePinModelToStorageAsync(PinModel pinModel);
        Task<bool> UpdatePinModelToStorageAsync(PinModel pinModel);

    }
}
