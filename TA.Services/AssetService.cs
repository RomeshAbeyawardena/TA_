using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts;
using TA.Domains.Dtos;
using WebToolkit.Contracts.Providers;

namespace TA.Services
{
    public class AssetService : IAssetService
    {
        private readonly IMapperProvider _mapperProvider;
        private readonly IRepository<Domains.Models.Asset> _assetRepository;

        private Asset Map(Domains.Models.Asset asset)
        {
            return _mapperProvider.Map<Domains.Models.Asset, Asset>(asset);
        }

        private IEnumerable<Asset> Map(IEnumerable<Domains.Models.Asset> asset)
        {
            return _mapperProvider.Map<Domains.Models.Asset, Asset>(asset);
        }
        
        private Domains.Models.Asset Map(Asset asset)
        {
            return _mapperProvider.Map<Asset, Domains.Models.Asset>(asset);
        }

        public async Task<IEnumerable<Asset>> GetAssets(Site site, bool showInActive = false)
        {
            return Map(await _assetRepository.
                Query().Where(asset => asset.SiteId != site.Id || !showInActive || site.Active).ToArrayAsync());
        }

        public async Task<Asset> GetAsset(Site site, string key, bool trackEntity)
        {
            return Map(await _assetRepository.
                Query(trackingQuery: trackEntity).FirstOrDefaultAsync(asset =>
                asset.SiteId == site.Id && asset.Key == key));
        }

        public async Task<Asset> GetAsset(int assetId)
        {
            return Map(await _assetRepository.DbSet.FindAsync(assetId));
        }

        public async Task<Asset> SaveAsset(Asset asset, bool saveChanges = true)
        {
            return Map(await _assetRepository.SaveChangesAsync(Map(asset), saveChanges));
        }

        public AssetService(IMapperProvider mapperProvider, IRepository<Domains.Models.Asset> assetRepository)
        {
            _mapperProvider = mapperProvider;
            _assetRepository = assetRepository;
        }
    }
}