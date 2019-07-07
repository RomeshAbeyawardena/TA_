using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts.Services;
using TA.Domains.Models;
using WebToolkit.Contracts.Data;
using WebToolkit.Contracts.Providers;

namespace TA.Services
{
    public class AssetService : IAssetService
    {
        private readonly IMapperProvider _mapperProvider;
        private readonly IRepository<Domains.Models.Asset> _assetRepository;

        public async Task<IEnumerable<Asset>> GetAssets(Site site, bool showAll = false)
        {
            return await _assetRepository.
                Query(asset => asset.SiteId == site.Id 
                               && showAll || site.IsActive)
                .Include(asset => asset.Site)
                .ToArrayAsync();
        }

        public async Task<Asset> GetAsset(Site site, string key, bool trackEntity)
        {
            return await _assetRepository.
                Query(trackingQuery: trackEntity).FirstOrDefaultAsync(asset =>
                asset.SiteId == site.Id && asset.Key == key);
        }

        public async Task<Asset> GetAsset(int assetId)
        {
            return await _assetRepository.DbSet.FindAsync(assetId);
        }

        public async Task<Asset> SaveAsset(Asset asset, bool saveChanges = true)
        {
            return (await _assetRepository.SaveChangesAsync(asset, saveChanges));
        }

        public AssetService(IMapperProvider mapperProvider, IRepository<Domains.Models.Asset> assetRepository)
        {
            _mapperProvider = mapperProvider;
            _assetRepository = assetRepository;
        }
    }
}