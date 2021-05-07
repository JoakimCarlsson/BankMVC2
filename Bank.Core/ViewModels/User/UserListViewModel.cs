using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.ViewModels.User
{
    public class UserListViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
