using Acr.UserDialogs;
using GeoTimeZone;
using System;
using TimeZoneConverter;
using Xamarin.Forms.GoogleMaps;

namespace GpsNotepad.Services.TimeZone
{
    public class TimeZoneService : ITimeZoneService
    {
        #region    ---   Methods   ---
        public DateTimeOffset GetCurrentTime(Position position)
        {
            DateTimeOffset convertedTime;
            try
            {
                string tz = TimeZoneLookup.GetTimeZone(position.Latitude, position.Longitude).Result;
                TimeZoneInfo tzInfo = TZConvert.GetTimeZoneInfo(tz);
                convertedTime = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tzInfo);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }

            return convertedTime;
        }

        public TimeZoneInfo GetTypeTime(Position position)
        {
            TimeZoneInfo tzInfo = null;
            try
            {
                string tz = TimeZoneLookup.GetTimeZone(position.Latitude, position.Longitude).Result;
                tzInfo = TZConvert.GetTimeZoneInfo(tz);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }

            return tzInfo;
        }

        #endregion
    }
}
