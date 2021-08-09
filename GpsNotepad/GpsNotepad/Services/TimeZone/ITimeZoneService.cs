using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Services.TimeZone
{
    public interface ITimeZoneService
    {
        DateTimeOffset GetCurrentTime(Position position);
        TimeZoneInfo GetTypeTime(Position position);

    }
}
