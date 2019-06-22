using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TA.Domains.Models
{
    public class Site
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string JsonAttributes { get; set; }
        public bool Active { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
        
        [NotMapped]
        public JObject Attributes
        {
            get =>  JObject.Parse(JsonAttributes);
            set => JsonAttributes = value.ToString(Formatting.None);
        }
    }
}