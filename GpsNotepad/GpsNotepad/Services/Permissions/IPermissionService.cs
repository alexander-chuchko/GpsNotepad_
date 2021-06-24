using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Permissions
{
    public interface IPermissionService
    {
        Task<bool> GetPermissionAsync(Permission permission);
        void SetStatusPermission(bool value);
        bool GetStatusPermission();
    }
}
