using System.Threading.Tasks;

namespace TA.Contracts
{
    public interface IPermissionService
    {
        Task<Domains.Models.Permission> GetPermissionByName(string permissionName);
    }
}