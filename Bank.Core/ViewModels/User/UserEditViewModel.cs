using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bank.Core.ViewModels.User
{
    public class UserEditViewModel : UserBaseViewModel
    {
        public string Id { get; set; }
        public string OldEmail { get; set; }
        public List<string> CurrentRoles { get; set; }
        public List<SelectListItem> AllRoles { get; set; }
    }
}
