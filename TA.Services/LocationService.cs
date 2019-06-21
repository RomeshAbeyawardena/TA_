using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts;
using TA.Domains.Models;

namespace TA.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<Location> _locationRepository;

        public async Task<Location> GetLocation()
        {
            return await _locationRepository.DbSet.FirstOrDefaultAsync();
        }

        public LocationService(IRepository<Location> locationRepository)
        {
            _locationRepository = locationRepository;
        }
    }
}