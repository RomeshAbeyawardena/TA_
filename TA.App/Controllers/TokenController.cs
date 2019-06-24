using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.App.Attributes;
using TA.App.ViewModels;
using TA.Contracts;
using TA.Domains.Enumerations;
using TA.Domains.Extensions;
using TA.Domains.Models;
using Permission = TA.Contracts.Permission;

namespace TA.App.Controllers
{
    [RequiresApiKey(Permission.TokenManager)]
    public class TokenController : ControllerBase
    {
        private readonly IApplicationSettings _applicationSettings;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITokenService _tokenService;
        private readonly ITokenKeyGenerator _tokenKeyGenerator;
        private readonly IPermissionService _permissionService;

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
                var permission = await _permissionService.GetPermissionByName(permissionName);

                tokenPermissionList.Add(new TokenPermission
                {
                    PermissionId = permission.Id,
                    Created = dateNow,
                    Expires = expiryDate
                });
            }

            return tokenPermissionList;
        }

        public TokenController(IApplicationSettings applicationSettings, IDateTimeProvider dateTimeProvider, 
            ITokenService tokenService, ITokenKeyGenerator tokenKeyGenerator, IPermissionService permissionService)
        {
            _applicationSettings = applicationSettings;
            _dateTimeProvider = dateTimeProvider;
            _tokenService = tokenService;
            _tokenKeyGenerator = tokenKeyGenerator;
            _permissionService = permissionService;
        }

        public async Task<ActionResult> AssignTokenPermissions([FromBody] GenerateTokenViewModel generateTokenViewModel)
        {
            var token = await _tokenService.GetToken(generateTokenViewModel.TokenKey);

            token.TokenPermissions = (await AssignTokenPermissions(generateTokenViewModel.Permissions)).ToArray();

            var savedToken = await _tokenService.SaveToken(token);
            return Ok(savedToken);
        }

        public async Task<ActionResult> GenerateToken([FromBody] GenerateTokenViewModel generateTokenViewModel)
        {
            var expiryDate = DetermineExpiryDate();

            var tokenKey = _tokenKeyGenerator.GenerateKey(HashAlgorithm.Sha512);
            var generatedToken = _tokenService.GenerateToken(tokenKey, expiryDate);

            if (generateTokenViewModel.Permissions.Any())
                generatedToken.TokenPermissions = (await AssignTokenPermissions(generateTokenViewModel.Permissions)).ToArray();
                
            var savedToken = await _tokenService.SaveToken(generatedToken);
            return Ok(savedToken);
        }
    }
}