using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bank.Web.Services.User
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
        public Task DeleteUserAsync(string userId);
    }
}
