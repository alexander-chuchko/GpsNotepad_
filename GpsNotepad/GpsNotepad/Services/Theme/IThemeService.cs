using GpsNotepad.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNotepad.Services.Theme
{
    public interface IThemeService
    {
        void PerformThemeChange(EnumSet.Theme theme);
        EnumSet.Theme GetValueTheme();
        void SetValueTheme(EnumSet.Theme themType);
    }
}
