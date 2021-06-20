using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Permissions
{
    public interface IPermissionService
    {
        Task<bool> GetPermissionAsync(Permission permission);
    }
}
