using System.Threading.Tasks;
using TA.Domains.Dtos;

namespace TA
{
    public interface ISiteService
    {
        Task<Site> GetSite(string name);
        Task<Site> GetSite(int id);
        Task<Site> SaveSite(Site site);
    }
}