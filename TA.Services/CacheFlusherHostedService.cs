using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TA.Contracts.Services;
using TA.Domains.Constants;
using TA.Domains.Models;
using WebToolkit.Contracts.Providers;

namespace TA.Services
{
    public class CacheFlusherHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<CacheFlusherHostedService> _logger;
        private readonly ITokenService _tokenService;
        private readonly ICacheProvider _cacheProvider;
        private readonly System.Timers.Timer _timer;

        private void Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var hasChanged = HasEntryChanged<Token, int>(_tokenService.GetTokenMaxId(),
                Caching.TokenPermissionCacheKey,
                token => token.Id, (a, b) => a > b).Result;

            if (!hasChanged)
            {
                _logger.LogInformation("{0:dd/MM/yyyy hh:mm}: No changes!", DateTimeOffset.Now);
                return;
            }

            _logger.LogInformation("Clearing {0}", Caching.TokenPermissionCacheKey);
            _cacheProvider.ClearByKey(Caching.TokenPermissionCacheKey).Wait();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.Start();
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            await Task.CompletedTask;
        }

        public async Task<bool> HasEntryChanged<TModel, TSelector>(Task<TSelector> sourceExpression,
            string cacheKeyName, Func<TModel, TSelector> selector, Func<TSelector, TSelector, bool> selectorComparer)
        {
            var max = await sourceExpression;
            var local = await _cacheProvider.Get<IEnumerable<TModel>>(CacheType.DistributedCache, cacheKeyName);

            if (local == null)
                return false;

            var maxLocal = local.Max(selector);
            return selectorComparer(max, maxLocal);
        }

        public CacheFlusherHostedService(ILogger<CacheFlusherHostedService> logger, ITokenService tokenService, ICacheProvider cacheProvider)
        {
            _logger = logger;
            _tokenService = tokenService;
            _cacheProvider = cacheProvider;
            _timer = new System.Timers.Timer(60000);
            _timer.Elapsed += Elapsed;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}