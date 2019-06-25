using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Domains.Dtos;

namespace TA
{
    public interface ISiteService
    {
        Task<Site> GetSite(string name, bool trackEntity);
        Task<Site> GetSite(int id);
        Task<Site> SaveSite(Site site, bool saveChanges = true);
        Task<IEnumerable<Site>> GetSites(bool showInActive = false);
    }
}