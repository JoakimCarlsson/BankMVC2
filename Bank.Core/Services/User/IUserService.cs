using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Core.ViewModels.User;
using Bank.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Services.User
{
    public interface IUserService
    {
        public Task<bool> CheckIfEmailExists(string email);
        public Task<UserListViewModel> GetAllUsersAsync();
    }

    class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        //todo maybe implement Repo
        public UserService(UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> CheckIfEmailExists(string email)
        {
            //return await _dbContext.Users.FirstOrDefaultAsync(i => i.Email == email).ConfigureAwait(false) != null;
            return false;
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
                    userViewModel.Roles.Add(role );
                }
                users.Add(userViewModel);
            }

            model.Users = users;
            return model;
        }
    }
}
