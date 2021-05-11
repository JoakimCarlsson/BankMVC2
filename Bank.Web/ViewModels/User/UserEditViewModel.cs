using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bank.Web.ViewModels.User
{
    public class UserEditViewModel : UserBaseViewModel
    {
        public string Id { get; set; }
        public string OldEmail { get; set; }
        public List<string> CurrentRoles { get; set; }
        public List<SelectListItem> AllRoles { get; set; }
    }
}
