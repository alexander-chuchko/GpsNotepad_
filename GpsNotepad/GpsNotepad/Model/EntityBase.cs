using SQLite;

namespace GpsNotepad.Model
{
    public abstract class EntityBase:IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
 