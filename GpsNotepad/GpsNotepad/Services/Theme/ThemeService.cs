using GpsNotepad.Enum;
using GpsNotepad.Service.Settings;
using ProfileBook.Styles;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GpsNotepad.Services.Theme
{
    public class ThemeService : IThemeService
    {
        private readonly ISettingsManager _settingsManager;
        public ThemeService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }
        public void SetValueTheme(EnumSet.Theme theme)
        {
            _settingsManager.ThemType = (int)theme;
        }
        public EnumSet.Theme GetValueTheme()
        {
            return (EnumSet.Theme)_settingsManager.ThemType;
        }
        public void PerformThemeChange(EnumSet.Theme theme)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                switch (theme)
                {
                    case EnumSet.Theme.Dark:
                        mergedDictionaries.Add(new DarkTheme());
                        break;
                    case EnumSet.Theme.Light:
                    default:
                        mergedDictionaries.Add(new LightTheme());
                        break;
                }
            }
        }
    }
}
