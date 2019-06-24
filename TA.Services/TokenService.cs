using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts;
using TA.Domains.Models;
using Permission = TA.Contracts.Permission;

namespace TA.Services
{
    public class TokenService : ITokenService
    {
        private readonly IRepository<Token> _tokenRepository;
        private readonly IRepository<TokenPermission> _tokenPermissionRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public async Task<bool> IsValid(Token token)
        {
            return await _tokenRepository.NoTrackingQuery.AnyAsync(t => t.Key == token.Key 
                                                                        && t.Expires > _dateTimeProvider.Now.DateTime);
        }

        public Token ClearTokenPermissions(Token token)
        {
            _tokenRepository.DbSet.Attach(token);
            token.TokenPermissions.Clear();
            return token;
        }

        public async Task<Token> GetToken(string tokenKey)
        {
            return await _tokenRepository.NoTrackingQuery
                .Include(token => token.TokenPermissions)
                .SingleOrDefaultAsync(token => token.Key == tokenKey);
        }


        public async Task<bool> HasPermission(Token token, Permission permission)
        {
            return await _tokenPermissionRepository.NoTrackingQuery.AnyAsync(tp => tp.TokenId == token.Id 
                                                                                   && tp.PermissionId == (int)permission
                                                                                   && tp.Expires > _dateTimeProvider.Now);
        }

        public async Task<bool> HasPermissions(Token token, IEnumerable<Permission> permissions)
        {
            foreach (var permission in permissions)
            {
                if (!await HasPermission(token, permission))
                    return false;
            }

            return true;
        }

        public Token GenerateToken(string tokenKey, DateTimeOffset expiryDate)
        {
            return new Token
            {
                Expires = expiryDate,
                IsActive = true,
                Key = tokenKey
            };
        }

        public async Task<Token> SaveToken(Token generatedToken)
        {
            return await _tokenRepository.SaveChangesAsync(generatedToken);
        }

        public TokenService(IRepository<Token> tokenRepository, IRepository<TokenPermission> tokenPermissionRepository, IDateTimeProvider dateTimeProvider)
        {
            _tokenRepository = tokenRepository;
            _tokenPermissionRepository = tokenPermissionRepository;
            _dateTimeProvider = dateTimeProvider;
        }
    }
}