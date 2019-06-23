using System.ComponentModel.DataAnnotations;

namespace TA.App.ViewModels
{
    public class AssetViewModel
    {
        public int Id { get; set; }
        [Required]
        public string SiteName { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string RelativeUrl { get; set; }

        [Required]
        public string JsonAttributes { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}