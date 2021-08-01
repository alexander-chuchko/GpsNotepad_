using SQLite;

namespace GpsNotepad.Model
{
    [Table(nameof(ImagesPin))]
    public class ImagesPin: EntityBase
    {
        public string PathImage { get; set; }
        public int PinId { get; set; }
    }
}
