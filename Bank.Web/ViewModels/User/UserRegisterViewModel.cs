using System.Collections.Generic;

namespace Bank.Web.ViewModels.User
{
    public class UserRegisterViewModel : UserBaseViewModel
    {
        public string ConfirmEmail { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
