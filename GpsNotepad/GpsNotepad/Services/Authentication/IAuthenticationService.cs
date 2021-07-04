using GpsNotepad.Model;
using System.Threading.Tasks;

namespace GpsNotepad.Service
{
    public interface IAuthenticationService
    {
        Task<UserModel> SignUpAsync(string email, string password, string name);
        Task<bool> SignInAsync(string email, string password);
    }
}
