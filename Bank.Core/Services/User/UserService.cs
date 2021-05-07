using AutoMapper;
using Bank.Core.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Core.Services.User
{
    class UserService : IUserService
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

        public async Task<UserEditViewModel> GetUserEdit(string id)
        {
            return _mapper.Map<UserEditViewModel>(_userManager.Users.First(i => i.Id == id));
        }

        public async Task<UserViewModel> GetUserByIdAsync(string id)
        {
            return _mapper.Map<UserViewModel>(await _userManager.Users.FirstAsync(i => i.Id == id).ConfigureAwait(false));
        }

        public async Task<List<SelectListItem>> GetUserRoles(string id)
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

        public async Task SaveUserAsync(UserEditViewModel model)
        {
            //todo, we can probaly fix this. We ew can just check which roles that has been changed and fix that accodinly.
            //but this also works..

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