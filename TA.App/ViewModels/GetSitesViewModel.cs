using System.ComponentModel.DataAnnotations;

namespace TA.App.ViewModels
{
    public class GetSitesViewModel
    {
        [Required]
        public bool ShowInActive { get; set; }
    }
}