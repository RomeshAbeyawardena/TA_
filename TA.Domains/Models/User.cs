using System;
using WebToolkit.Contracts;

namespace TA.Domains.Models
{
    public class User : IIdentity, ICreated, IModified
    {
        public int Id { get; set; }
        public Guid UserToken { get; set; }
        public byte[] EmailAddress { get; set; }
        public byte[] Password { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
    }
}