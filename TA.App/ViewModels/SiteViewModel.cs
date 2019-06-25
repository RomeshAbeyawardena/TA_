using System.ComponentModel.DataAnnotations;

namespace TA.App.ViewModels
{
    public class SiteViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public bool IsActive { get; set; }
        
        public object Attributes { get; set; }
        [Required]
        public bool Active { get; set; }

    }
}