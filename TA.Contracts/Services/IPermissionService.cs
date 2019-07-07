using System.Collections.Generic;
using System.Threading.Tasks;

namespace TA.Contracts.Services
{
    public interface IPermissionService
    {
        Domains.Models.Permission GetPermissionByName(string permissionName,
            IEnumerable<Domains.Models.Permission> permissions);
        Task<IEnumerable<Domains.Models.Permission>> GetPermissions(bool showAll = false);
    }
}