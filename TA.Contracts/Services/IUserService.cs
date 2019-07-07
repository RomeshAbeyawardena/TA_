using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Domains.Models;

namespace TA.Contracts.Services
{
    public interface IUserService
    {
        Task<User> SaveUser(User user, bool hashPassword = true, bool commitChanges = true);
        Task<IEnumerable<User>> GetUsers(bool showAll = false);
        User GetUser(IEnumerable<User> users, int id);
        User GetUser(IEnumerable<User> users, Guid token);
        User GetUser(IEnumerable<User> users, string emailAddress);
        bool IsValid(IEnumerable<User> users, string emailAddress, string password);
    }
}