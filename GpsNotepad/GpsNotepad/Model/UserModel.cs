using SQLite;

namespace GpsNotepad.Model
{
    [Table(nameof(UserModel))]
    public class UserModel : EntityBase
    {

        public string Name { get; set; }

        [Unique]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
