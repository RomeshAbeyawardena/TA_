using System.Threading.Tasks;
using TA.Domains.Models;

namespace TA
{
    public interface ISiteService
    {
        Task<Site> GetSite();
        Task<Site> SaveSite(Site site);
    }
}