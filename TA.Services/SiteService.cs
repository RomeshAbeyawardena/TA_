using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts;
using TA.Contracts.Services;
using TA.Domains.Dtos;
using WebToolkit.Contracts.Data;
using WebToolkit.Contracts.Providers;

namespace TA.Services
{
    public class SiteService : ISiteService
    {
        private readonly IMapperProvider _mapperProvider;
        private readonly IRepository<Domains.Models.Site> _siteRepository;

        private IEnumerable<Site> Map(IEnumerable<Domains.Models.Site> site)
        {
            return _mapperProvider.Map<Domains.Models.Site, Site>(site);
        }

        private Site Map(Domains.Models.Site site)
        {
            return _mapperProvider.Map<Domains.Models.Site, Site>(site);
        }

        private Domains.Models.Site Map(Site site)
        {
            return _mapperProvider.Map<Site, Domains.Models.Site>(site);
        }

        public SiteService(IMapperProvider mapperProvider, IRepository<Domains.Models.Site> siteRepository)
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
            return Map(await _siteRepository
                .DbSet.FindAsync(id));
        }

        public async Task<Site> SaveSite(Site site, bool saveChanges = true)
        {
            return Map(await _siteRepository
                .SaveChangesAsync(Map(site), saveChanges));
        }

        public async Task<IEnumerable<Site>> GetSites(bool showInActive = false)
        {
            return Map(await _siteRepository.Query().Where(a => !showInActive || a.IsActive).ToArrayAsync());
        }
    }
}