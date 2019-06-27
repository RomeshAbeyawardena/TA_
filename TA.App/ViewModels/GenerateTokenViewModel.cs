using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TA.App.ViewModels
{
    public class GenerateTokenViewModel
    {
        public string TokenKey { get; set; }
        [Required] public IEnumerable<string> Permissions { get; set; }
    }
}