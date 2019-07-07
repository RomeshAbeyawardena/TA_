using System;

namespace TA.App.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public Guid UserToken { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}