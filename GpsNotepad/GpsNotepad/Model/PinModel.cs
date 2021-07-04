using SQLite;

namespace GpsNotepad.Model
{
    [Table(nameof(PinModel))]
    public class PinModel:EntityBase
    {
        public int UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Unique]
        public string Label { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public bool Favorite { get; set; }
    }
}
