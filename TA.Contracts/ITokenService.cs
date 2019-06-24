using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Domains.Models;

namespace TA.Contracts
{
    public enum Permission { 
        Create = 1, 
        Read = 2, 
        Update = 3, 
        SoftDelete = 4, 
        Delete = 5, 
        TokenManager = 6
    }

    public interface ITokenService
    {
        Token ClearTokenPermissions(Token token);
        Task<Token> GetToken(string tokenKey);
        Task<bool> IsValid(Token token);
        Task<bool> HasPermission(Token token, Permission permission);
        Task<bool> HasPermissions(Token token, IEnumerable<Permission> permission);
        Token GenerateToken(string tokenKey, DateTimeOffset expiryDate);
        Task<Token> SaveToken(Token generatedToken);
    }
}