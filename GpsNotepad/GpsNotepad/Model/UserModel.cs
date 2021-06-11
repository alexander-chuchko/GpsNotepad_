using SQLite;

namespace GpsNotepad.Model
{
    [Table(nameof(UserModel))]
    public class UserModel : EntityBase
    {
        [Unique]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
