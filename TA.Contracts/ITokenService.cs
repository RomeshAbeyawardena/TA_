using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Domains.Models;

namespace TA.Contracts
{
    public enum Permission { Create, Read, Update, Delete }
    public interface ITokenService
    {
        Task<Token> GetToken(string tokenKey);
        Task<bool> IsValid(Token token);
        Task<bool> HasPermission(Token token, Permission permission);
        Task<bool> HasPermissions(Token token, IEnumerable<Permission> permission);
    }
}