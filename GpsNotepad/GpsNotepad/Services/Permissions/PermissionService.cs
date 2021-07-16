using Acr.UserDialogs;
using GpsNotepad.Service.Settings;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Permissions
{
    public class PermissionService: IPermissionService
	{
		private readonly IUserDialogs _userDialogs;
		private readonly ISettingsManager _settingsManager;
		public PermissionService(IUserDialogs userDialogs, ISettingsManager settingsManager)
        {
			_userDialogs = userDialogs;
			_settingsManager = settingsManager;
		}
		public void SetStatusPermission(bool value)
        {
			_settingsManager.IsEnabledUserLocationButton=value;
        }
		public bool GetStatusPermission()
        {
			return _settingsManager.IsEnabledUserLocationButton;
        }
		public async Task<bool> GetPermissionAsync(Permission permission)
		{
			bool result = false;
			try
			{
				//Проверяем статус разрешения
				PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
				if (status != PermissionStatus.Granted)
				{
					//Для чего выполняем запрос
					if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
					{
						await _userDialogs.AlertAsync("Need location", "Gunna need that location", "OK");
						//status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
					}
					//
					status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
				}
				result = status == PermissionStatus.Granted;
			}
			catch(Exception ex)
			{
				_userDialogs.Alert(ex.Message);
			}
			return result;
		}
	}
}
