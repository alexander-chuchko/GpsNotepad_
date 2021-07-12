using Prism.Mvvm;
using System;

namespace GpsNotepad.Model.Pin
{
    public class PinViewModel : BindableBase
    {
        private int _id;
        private int _userId;
        private double _latitude;
        private double _longitude;
        private string _label;
        private string _address;
        private string _description;
        public bool _favorite;
        public string _imagePath;

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value ); }
        }
        public int UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }
        public double Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value); }
        }
        public double Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value); }
        }
        public string Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        public bool Favorite
        {
            get { return _favorite; }
            set { SetProperty(ref _favorite, value); }
        }
        public string ImagePath
        {
            get { return _imagePath; }
            set { SetProperty(ref _imagePath, value); }
        }
    }
}
