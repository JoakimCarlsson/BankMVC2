using AutoMapper;
using Bank.Web.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Web.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<IdentityUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<bool> CheckIfEmailExistsAsync(string email)
        {
            return await _userManager.Users.FirstOrDefaultAsync(i => i.Email == email).ConfigureAwait(false) == null;
        }

        public async Task<UserListViewModel> GetAllUsersAsync()
        {
            var model = new UserListViewModel();
            List<UserViewModel> users = new List<UserViewModel>();

            foreach (IdentityUser user in _userManager.Users)
            {
                var userViewModel = _mapper.Map<UserViewModel>(user);
                userViewModel.Roles = new List<string>();
                var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

                foreach (string role in roles)
                {
                    userViewModel.Roles.Add(role);
                }
                users.Add(userViewModel);
            }

            model.Users = users;
            return model;
        }

        public  Task<UserEditViewModel> GetUserEdit(string id)
        {
            return Task.FromResult(_mapper.Map<UserEditViewModel>(_userManager.Users.First(i => i.Id == id)));
        }

        public async Task<UserViewModel> GetUserByIdAsync(string id)
        {
            return _mapper.Map<UserViewModel>(await _userManager.Users.FirstAsync(i => i.Id == id).ConfigureAwait(false));
        }

        public async Task<List<SelectListItem>> GetActiveUserRoles(string id)
        {
            var roles = _roleManager.Roles.ToList();
            var activeRoles = await _userManager.GetRolesAsync(_userManager.Users.First(i => i.Id == id)).ConfigureAwait(false);
            var list = new List<SelectListItem>();

            list.AddRange(roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name,
                Selected = activeRoles.Contains(r.Name)
            }));

            return list;
        }

        public Task<List<string>> GetAllRolesAsync()
        {
            List<string> tmpList = new();
            tmpList.AddRange(_roleManager.Roles.Select(i => i.Name));
            return Task.FromResult(tmpList);
        }

        public async Task SaveUserAsync(UserBaseViewModel model)
        {
            switch (model)
            {
                case UserEditViewModel viewModel:
                    await SaveEditAsync(viewModel).ConfigureAwait(false);
                    break;
                case UserRegisterViewModel viewModel:
                    await RegisterNewUserAsync(viewModel).ConfigureAwait(false);
                    break;
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(i => i.Id == userId);
            await _userManager.DeleteAsync(user).ConfigureAwait(false);
        }

        private async Task RegisterNewUserAsync(UserRegisterViewModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);

            if (result.Succeeded)
            {
                foreach (string role in model.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role).ConfigureAwait(false);
                }
            }
        }

        private async Task SaveEditAsync(UserEditViewModel model)
        {
            var user = _userManager.Users.FirstOrDefault(i => i.Id == model.Id);
            await _userManager.RemoveFromRolesAsync(user, (List<string>)await _userManager.GetRolesAsync(user)).ConfigureAwait(false);
            await _userManager.AddToRolesAsync(user, model.CurrentRoles).ConfigureAwait(false);

            if (model.OldEmail != model.Email)
            {
                await _userManager.SetEmailAsync(user, model.Email).ConfigureAwait(false);
                await _userManager.SetUserNameAsync(user, model.Email).ConfigureAwait(false);
            }
        }
    }
}