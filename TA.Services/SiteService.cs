using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts;
using TA.Domains.Dtos;
using WebToolkit.Contracts.Providers;

namespace TA.Services
{
    public class SiteService : ISiteService
    {
        private readonly IMapperProvider _mapperProvider;
        private readonly IRepository<Domains.Models.Site> _siteRepository;

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


        public async Task<Site> GetSite(string name)
        {
            return Map(await _siteRepository.
                NoTrackingQuery.FirstOrDefaultAsync(site => site.Name == name));
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
    }
}