using System.Collections.Generic;

namespace TA.App.ViewModels
{
    public class GenerateTokenViewModel
    {
        public string TokenKey { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}