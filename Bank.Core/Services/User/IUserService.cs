using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Core.ViewModels.User;
using Bank.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Services.User
{
    public interface IUserService
    {
        public Task<bool> CheckIfEmailExistsAsync(string email);
        public Task<UserListViewModel> GetAllUsersAsync();
        public Task<UserEditViewModel> GetUserEdit(string id);
        public Task<UserViewModel> GetUserByIdAsync(string id);
        public Task<List<SelectListItem>> GetActiveUserRoles(string id);
        public Task<List<string>> GetAllRolesAsync();
        public Task SaveUserAsync(UserBaseViewModel model);
    }
}
