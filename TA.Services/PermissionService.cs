using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts;
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

        public async Task<Permission> GetPermissionByName(string permissionName)
        {
            return await _permissionRepository.Query().FirstOrDefaultAsync(permission =>
                permission.Name == permissionName);
        }
    }
}