using AutoMapper;
using Bank.Data.Models;
using Bank.Web.ViewModels.Accounts;

namespace Bank.Web.Mapping
{
    public class AccountProfiles : Profile
    {
        public AccountProfiles()
        {
            CreateMap<Account, AccountCustomerViewModel>().ReverseMap();
        }
    }
}