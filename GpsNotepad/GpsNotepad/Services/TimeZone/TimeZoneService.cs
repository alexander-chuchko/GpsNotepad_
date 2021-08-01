using GeoTimeZone;
using System;
using System.Collections.Generic;
using System.Text;
using TimeZoneConverter;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Services.TimeZone
{
    public class TimeZoneService: ITimeZoneService
    {

        public TimeZoneService()
        {

        }

        public void GetCurrentTime(Position position)
        {

            string tz = TimeZoneLookup.GetTimeZone(position.Latitude, position.Longitude).Result;
            TimeZoneInfo tzInfo = TZConvert.GetTimeZoneInfo(tz);
            DateTimeOffset convertedTime = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tzInfo);

        }
    }
}
