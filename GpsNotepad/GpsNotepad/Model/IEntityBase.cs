using SQLite;

namespace GpsNotepad.Model
{
    public interface IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        int Id { get; set; }
    }
}
