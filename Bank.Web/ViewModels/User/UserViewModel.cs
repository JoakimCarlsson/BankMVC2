using System.Collections.Generic;

namespace Bank.Web.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; }
    }
}