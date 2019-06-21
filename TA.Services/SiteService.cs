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

        public async Task<Site> GetSite()
        {
            return await _siteRepository.DbSet.FirstOrDefaultAsync();
        }
    }
}