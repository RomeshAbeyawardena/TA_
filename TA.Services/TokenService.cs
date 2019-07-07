using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts.Services;
using TA.Domains.Models;
using WebToolkit.Contracts.Data;
using WebToolkit.Contracts.Providers;
using Permission = TA.Contracts.Services.Permission;

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

        public Token GetToken(IEnumerable<Token> tokens, string tokenKey)
        {
            return tokens
                .SingleOrDefault(token => token.Key == tokenKey);
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

        public async Task<IEnumerable<Token>> GetTokens(bool showAll = false)
        {
            return await _tokenRepository.Query(token => showAll 
                                                         || token.IsActive 
                                                         && token.Expires > _dateTimeProvider.Now)
                .Include(token => token.TokenPermissions)
                .ToArrayAsync();
        }


        public async Task<int> GetTokenMaxId()
        {
            return await _tokenRepository.Query().MaxAsync(a => a.Id);
        }

        public TokenService(IRepository<Token> tokenRepository, IDateTimeProvider dateTimeProvider)
        {
            _tokenRepository = tokenRepository;
            _dateTimeProvider = dateTimeProvider;
        }
    }
}