using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts.Services;
using WebToolkit.Contracts.Data;
using Permission = TA.Domains.Models.Permission;

namespace TA.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepository<Permission> _permissionRepository;

        public PermissionService(IRepository<Permission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public Permission GetPermissionByName(string permissionName, IEnumerable<Permission> permissions)
        {
            return permissions.FirstOrDefault(permission =>
                permission.Name == permissionName);
        }

        public async Task<IEnumerable<Permission>> GetPermissions()
        {
            return await _permissionRepository.Query().ToArrayAsync();
        }
    }
}