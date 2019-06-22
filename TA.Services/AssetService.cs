using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts;
using TA.Domains.Models;

namespace TA.Services
{
    public class AssetService : IAssetService
    {
        private readonly IRepository<Asset> _assetRepository;

        public async Task<IEnumerable<Asset>> GetAssets(Site site)
        {
            return await _assetRepository.DbSet.Where(asset => asset.SiteId == site.Id).ToArrayAsync();
        }

        public async Task<Asset> GetAsset(Site site, string key)
        {
            return await _assetRepository.DbSet.FirstOrDefaultAsync(asset =>
                asset.SiteId == site.Id && asset.Key == key);
        }

        public async Task<Asset> GetAsset(int assetId)
        {
            return await _assetRepository.DbSet.FindAsync(assetId);
        }

        public async Task<Asset> SaveAsset(Asset asset)
        {
            return await _assetRepository.SaveChangesAsync(asset, a => a.Id);
        }

        public AssetService(IRepository<Asset> assetRepository)
        {
            _assetRepository = assetRepository;
        }
    }
}