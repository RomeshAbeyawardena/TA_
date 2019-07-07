using System.ComponentModel.DataAnnotations;

namespace TA.App.ViewModels
{
    public class GetAssetsViewModel
    {
        [Required]
        public string SiteName { get; set; }

        [Required]
        public bool ShowAll { get; set; }
    }
}