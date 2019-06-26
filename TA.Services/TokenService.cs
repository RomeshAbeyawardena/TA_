using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts;
using TA.Contracts.Providers;
using TA.Domains.Models;
using Permission = TA.Contracts.Permission;

namespace TA.Services
{
    public class TokenService : ITokenService
    {
        private readonly IRepository<Token> _tokenRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public async Task<bool> IsValid(Token token)
        {
            return await _tokenRepository.Query().AnyAsync(t => t.Key == token.Key
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
            return await _tokenRepository.Query()
                .Include(token => token.TokenPermissions)
                .SingleOrDefaultAsync(token => token.Key == tokenKey);
        }


        public bool HasPermission(Token token, Permission permission)
        {
            return token.TokenPermissions
                .Any(tp => tp.TokenId == token.Id
                           && tp.PermissionId == (int) permission
                           && tp.Expires > _dateTimeProvider.Now);
        }

        public bool HasPermissions(Token token, IEnumerable<Permission> permissions)
        {
            return permissions.All(permission => HasPermission(token, permission));
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

        public TokenService(IRepository<Token> tokenRepository, IDateTimeProvider dateTimeProvider)
        {
            _tokenRepository = tokenRepository;
            _dateTimeProvider = dateTimeProvider;
        }
    }
}