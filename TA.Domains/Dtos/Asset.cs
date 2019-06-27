using System;
using Newtonsoft.Json.Linq;

namespace TA.Domains.Dtos
{
    public class Asset
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string Key { get; set; }
        public string RelativeUrl { get; set; }
        public bool IsActive { get; set; }
        public JObject Attributes { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }

        public virtual Site Site { get; set; }
    }
}