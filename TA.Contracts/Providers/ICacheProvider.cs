using System.Threading.Tasks;

namespace TA.Contracts.Providers
{
    public enum CacheType { DistributedCache }
    public interface ICacheProvider
    {
        Task<T> Get<T>(CacheType cacheType, string key);
        Task Set<T>(CacheType cacheType, string key, T value);
        
        Task<int> Clear();
    }
}