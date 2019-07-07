using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TA.Contracts.Services;
using TA.Domains.Constants;
using TA.Domains.Models;
using WebToolkit.Contracts.Data;
using WebToolkit.Contracts.Providers;
using Encoding = System.Text.Encoding;

namespace TA.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly ICryptographyProvider _cryptographyProvider;

        public async Task<User> SaveUser(User user, bool hashPassword = true, bool commitChanges = true)
        {
            if(hashPassword)
                user.Password = _cryptographyProvider
                    .HashBytes(HashAlgorithm.PasswordBytes, user.Password, Data.PasswordSalt)
                    .ToArray();

            user.EmailAddress =
                _cryptographyProvider
                    .Encrypt(Encoding.ASCII.GetString(user.EmailAddress), Data.Salt, Data.InitialVector, Encoding.ASCII).ToArray();
            return await _userRepository.SaveChangesAsync(user, commitChanges);
        }

        public async Task<IEnumerable<User>> GetUsers(bool showAll = false)
        {
            return await _userRepository.Query(user => showAll || user.IsActive).ToArrayAsync();
        }

        public User GetUser(IEnumerable<User> users, int id)
        {
            return users.SingleOrDefault(user => user.Id == id);
        }

        public User GetUser(IEnumerable<User> users, Guid token)
        {
            return users.SingleOrDefault(user => user.UserToken == token);
        }

        public User GetUser(IEnumerable<User> users, string emailAddress)
        {
            return users.SingleOrDefault(a =>
                _cryptographyProvider.Decrypt(a.EmailAddress, Data.Salt, Data.InitialVector, Encoding.ASCII) == emailAddress);

        }

        public bool IsValid(IEnumerable<User> users, string emailAddress, string password)
        {
            var foundUser = GetUser(users, emailAddress);
            var hashedPassword = _cryptographyProvider
                .HashBytes(HashAlgorithm.PasswordBytes, Encoding.ASCII.GetBytes(password), Data.PasswordSalt);
            return foundUser != null 
                   && hashedPassword.SequenceEqual(foundUser.Password);
        }

        public UserService(IRepository<User> userRepository, ICryptographyProvider cryptographyProvider)
        {
            _userRepository = userRepository;
            _cryptographyProvider = cryptographyProvider;
        }
    }
}