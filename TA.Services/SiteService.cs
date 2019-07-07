using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts.Services;
using TA.Domains.Models;
using WebToolkit.Contracts.Data;
using WebToolkit.Contracts.Providers;

namespace TA.Services
{
    public class SiteService : ISiteService
    {
        private readonly IMapperProvider _mapperProvider;
        private readonly IRepository<Site> _siteRepository;

        public SiteService(IMapperProvider mapperProvider, IRepository<Site> siteRepository)
        {
            _mapperProvider = mapperProvider;
            _siteRepository = siteRepository;
        }


        public Site GetSite(IEnumerable<Site> sites, string name)
        {
            return sites
                .FirstOrDefault(site => site.Name == name);
        }

        public async Task<Site> GetSite(int id)
        {
            return await _siteRepository
                .DbSet.FindAsync(id);
        }

        public async Task<Site> SaveSite(Site site, bool saveChanges = true)
        {
            return await _siteRepository
                .SaveChangesAsync(site, saveChanges);
        }

        public async Task<IEnumerable<Site>> GetSites(bool showAll = false)
        {
            return await _siteRepository.Query(a => showAll 
                                                        || a.IsActive).ToArrayAsync();
        }
    }
}