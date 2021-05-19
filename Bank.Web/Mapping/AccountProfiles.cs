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
            CreateMap<Account, AccountEditViewModel>().ReverseMap();
            CreateMap<Customer, AccountDisponentViewModel>();
            CreateMap<Disposition, AccountDisponentViewModel>()
                .ForMember(i => i.GivenName, opt => opt.MapFrom(i => i.Customer.Givenname))
                .ForMember(i => i.SurName, opt => opt.MapFrom(i => i.Customer.Surname));
        }
    }
}