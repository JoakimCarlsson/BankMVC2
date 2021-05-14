using AutoMapper;
using Bank.Web.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace Bank.Web.Mapping
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<IdentityUser, UserViewModel>();
            CreateMap<IdentityUser, UserEditViewModel>();
        }
    }
}