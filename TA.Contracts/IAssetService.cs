using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Domains.Dtos;

namespace TA.Contracts
{
    public interface IAssetService
    {
        Task<IEnumerable<Asset>> GetAssets(Site site);
        Task<Asset> GetAsset(Site site, string key);
        Task<Asset> GetAsset(int assetId);
        Task<Asset> SaveAsset(Asset asset, bool saveChanges = true);
    }
}