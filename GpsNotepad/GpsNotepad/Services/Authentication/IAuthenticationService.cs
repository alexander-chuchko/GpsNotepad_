using GpsNotepad.Model;
using System.Threading.Tasks;

namespace GpsNotepad.Service
{
    public interface IAuthenticationService
    {
        Task<UserModel> SignUpAsync(string login, string password);
        Task<bool> SignInAsync(string login, string password);
    }
}
