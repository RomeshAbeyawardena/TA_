using System.Threading.Tasks;

namespace TA
{
    public class App
    {
        private readonly ILocationService _locationService;

        public App(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<int> Begin()
        {
            await Task.Delay(100);
            _locationService.GetLocation();
            return 0;
        }
    }
}