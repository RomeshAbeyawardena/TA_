using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Domains.Models;

namespace TA.Contracts
{
    public interface IAssetService
    {
        Task<IEnumerable<Asset>> GetAssets(Site site);
        Task<Asset> GetAsset(Site site, string key);
        Task<Asset> SaveAsset(Asset asset);
    }
}