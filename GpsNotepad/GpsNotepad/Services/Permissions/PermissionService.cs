using Acr.UserDialogs;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Permissions
{
    public class PermissionService: IPermissionService
	{
		private readonly IUserDialogs UserDialogs;

        public PermissionService(IUserDialogs userDialogs)
        {
			UserDialogs = userDialogs;
		}
		public async Task<bool> GetPermissionAsync(Permission permission)
		{
			bool result = false;
			try
			{
				PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
				if (status != PermissionStatus.Granted)
				{
					if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
					{
						await UserDialogs.AlertAsync("Need location", "Gunna need that location", "OK");
					}
					status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
				}
				result = status == PermissionStatus.Granted;
			}
			catch(Exception ex)
			{
				await UserDialogs.AlertAsync(ex.Message); //Не забыть убрать await с блока catch
			}
			return result;
		}
	}
}
