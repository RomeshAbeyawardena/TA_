using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TA.App.Attributes;
using TA.App.ViewModels;
using TA.Contracts;
using TA.Contracts.Services;
using TA.Domains.Models;
using WebToolkit.Contracts.Providers;
using Permission = TA.Contracts.Services.Permission;

namespace TA.App.Controllers.Api
{
    [RequiresApiKey(Permission.TokenManager)]
    public class TokenController : ApiControllerBase
    {
        private readonly IApplicationSettings _applicationSettings;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITokenService _tokenService;
        private readonly ITokenKeyGenerator _tokenKeyGenerator;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<TokenController> _logger;
        private readonly INotificationHandler _notificationHandler;

        private DateTimeOffset DetermineExpiryDate()
        {
            return  _dateTimeProvider.Now.AddDays(_applicationSettings.DefaultTokenExpiryPeriodInDays
                                                  ?? Domains.Constants.Data.DefaultTokenExpiryPeriodInDays);
        }

        private async Task<IEnumerable<TokenPermission>> AssignTokenPermissions(IEnumerable<string> permissions)
        {
            var dateNow = _dateTimeProvider.Now;
            var expiryDate = DetermineExpiryDate();

            var tokenPermissionList = new List<TokenPermission>();

            foreach (var permissionName in permissions)
            {
                var permission = _permissionService.GetPermissionByName(permissionName, await Permissions);

                tokenPermissionList.Add(new TokenPermission
                {
                    PermissionId = permission.Id,
                    Created = dateNow,
                    Expires = expiryDate
                });
            }

            return tokenPermissionList;
        }

        public TokenController(ILogger<TokenController> logger, IApplicationSettings applicationSettings, IDateTimeProvider dateTimeProvider, 
            ITokenService tokenService, ITokenKeyGenerator tokenKeyGenerator, IPermissionService permissionService,
            INotificationHandler notificationHandler)
        {
            _logger = logger;
            _notificationHandler = notificationHandler;
            _notificationHandler.Subscribe(notification =>
            {
                _logger.LogInformation("{0}", notification.EventResult);
            }, nameof(TokenController));
            _applicationSettings = applicationSettings;
            _dateTimeProvider = dateTimeProvider;
            _tokenService = tokenService;
            _tokenKeyGenerator = tokenKeyGenerator;
            _permissionService = permissionService;
        }

        [HttpPost]
        public async Task<ActionResult> AssignTokenPermissions([FromBody] GenerateTokenViewModel generateTokenViewModel)
        {
            var token = _tokenService.GetToken(await Tokens, generateTokenViewModel.TokenKey);
            token = _tokenService.ClearTokenPermissions(token);
            token.TokenPermissions = (await AssignTokenPermissions(generateTokenViewModel.Permissions)).ToList();

            var savedToken = await _tokenService.SaveToken(token);
            await ClearTokenCache();
            return Ok(savedToken);
        }
        
        [HttpPost]
        public async Task<ActionResult> GenerateToken([FromBody] GenerateTokenViewModel generateTokenViewModel)
        {
            var expiryDate = DetermineExpiryDate();

            var tokenKey = _tokenKeyGenerator.GenerateKey(HashAlgorithm.Sha512);
            var generatedToken = _tokenService.GenerateToken(tokenKey, expiryDate);

            if (generateTokenViewModel.Permissions.Any())
                generatedToken.TokenPermissions = (await AssignTokenPermissions(generateTokenViewModel.Permissions)).ToArray();
                
            var savedToken = await _tokenService.SaveToken(generatedToken);
            await ClearTokenCache();
            return Ok(savedToken);
        }
    }
}