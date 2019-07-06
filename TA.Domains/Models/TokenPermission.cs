using System;
using System.ComponentModel.DataAnnotations;
using WebToolkit.Contracts;

namespace TA.Domains.Models
{
    public class TokenPermission : ICreated
    {
        [Key] public int Id { get; set; }
        public int PermissionId { get; set; }
        public int TokenId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Expires { get; set; }

        public virtual Token Token { get; set; }
        public virtual Permission Permission { get; set; }
    }
}