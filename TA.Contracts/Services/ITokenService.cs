using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Domains.Models;

namespace TA.Contracts.Services
{
    public enum Permission { 
        Create = 1, 
        Read = 2, 
        Update = 3, 
        SoftDelete = 4, 
        Delete = 5, 
        TokenManager = 6,
        ApiAccess = 7
    }

    public interface ITokenService
    {
        Token ClearTokenPermissions(Token token);
        Token GetToken(IEnumerable<Token> tokens, string tokenKey);
        Task<bool> IsValid(Token token);
        bool HasPermission(Token token, Permission permission);
        bool HasPermissions(Token token, IEnumerable<Permission> permission);
        Token GenerateToken(string tokenKey, DateTimeOffset expiryDate);
        Task<Token> SaveToken(Token generatedToken);
        Task<IEnumerable<Token>> GetTokens();
        Task<int> GetTokenMaxId();
    }
}