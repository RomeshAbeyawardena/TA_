using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

namespace TA.Domains.Models
{
    public class Asset
    {
        [Key] public int Id { get; set; }
        public int SiteId { get; set; }
        public string Key { get; set; }
        public string RelativePath { get; set; }
        public string JsonAttributes { get; set; }
        public int Active { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }

        public virtual Site Site { get; set; }

        [NotMapped]
        public JObject Attributes
        {
            get =>  JObject.Parse(JsonAttributes);
            set => JsonAttributes = value.ToString();
        }
}
}