using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TA.Domains.Contracts;

namespace TA.Domains.Models
{
    public class Token : ICreated
    {
        [Key] public int Id { get; set; }
        public string Key { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset Expires { get; set; }
        public DateTimeOffset Created { get; set; }

        public ICollection<TokenPermission> TokenPermissions { get; set; }
    }
}