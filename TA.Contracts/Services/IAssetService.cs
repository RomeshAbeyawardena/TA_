using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Domains.Models;

namespace TA.Contracts.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<Asset>> GetAssets(Site site, bool showInActive = false);
        Task<Asset> GetAsset(Site site, string key, bool trackEntity);
        Task<Asset> GetAsset(int assetId);
        Task<Asset> SaveAsset(Asset asset, bool saveChanges = true);
    }
}