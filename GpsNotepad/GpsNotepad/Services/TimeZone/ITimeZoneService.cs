using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Services.TimeZone
{
    public interface ITimeZoneService
    {
        void GetCurrentTime(Position position);

    }
}
