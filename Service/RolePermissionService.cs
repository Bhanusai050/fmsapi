using FmsAPI.Data;
using FmsAPI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FmsAPI.Service
{
    public class RolePermissionService: IRolePermissionService
    {
    FarmManagementSystemEnities context = new FarmManagementSystemEnities();

        public List<RolePermission> GetRolePermissions()
        {
            return context.RolePermissions.ToList();
        }
    }
}
