using System;
using System.ComponentModel.DataAnnotations;
using WebToolkit.Contracts;

namespace TA.Domains.Models
{
    public class Permission : ICreated, IModified
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
    }
}