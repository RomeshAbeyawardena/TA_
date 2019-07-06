using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Domains.Models;

namespace TA.Contracts.Services
{
    public interface ISiteService
    {
        Site GetSite(IEnumerable<Site> sites, string name);
        Task<Site> GetSite(int id);
        Task<Site> SaveSite(Site site, bool saveChanges = true);
        Task<IEnumerable<Site>> GetSites(bool showInActive = false);
    }
}