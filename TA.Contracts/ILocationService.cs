using System.Threading.Tasks;
using TA.Domains.Models;

namespace TA
{
    public interface ILocationService
    {
        Task<Location> GetLocation();
    }
}