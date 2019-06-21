using System.Threading.Tasks;

namespace TA
{
    public class App
    {
        private readonly ISiteService _siteService;

        public App(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<int> Begin()
        {
            var site = await _siteService.GetSite();
            return 0;
        }
    }
}