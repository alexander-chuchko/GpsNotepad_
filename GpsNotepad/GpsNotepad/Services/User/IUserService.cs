using GpsNotepad.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GpsNotepad.Service.User
{
    public interface IUserService
    {
        Task<bool> UpdateUserModelAsync(UserModel userModel);
        Task<bool> SaveUserModelAsync(UserModel userModel);
        Task<bool> DeleteUserModelAsync(UserModel userModel);
        Task<IEnumerable<UserModel>> GetAllUserModelAsync();
    }
}
