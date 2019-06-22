using System;
using Newtonsoft.Json.Linq;

namespace TA.Domains.Dtos
{
    public class Site
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
        public JObject Attributes { get; set; }
    }
}