using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts;
using TA.Domains.Models;

namespace TA.Services
{
    public class SiteService : ISiteService
    {
        private readonly IRepository<Site> _siteRepository;

        public SiteService(IRepository<Site> siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public async Task<Site> GetSite(string name)
        {
            return await _siteRepository
                .DbSet.FirstOrDefaultAsync(site => site.Name == name);
        }

        public async Task<Site> GetSite(int id)
        {
            return await _siteRepository
                .DbSet.FindAsync(id);
        }

        public async Task<Site> SaveSite(Site site)
        {
            return await _siteRepository
                .SaveChangesAsync(site, s => s.Id);
        }
    }
}