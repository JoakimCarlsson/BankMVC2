using AutoMapper;
using Bank.Data.Models;
using Bank.Web.ViewModels.Accounts;
using Bank.Web.ViewModels.Customers;
using Bank.Web.ViewModels.Statistics;
using Bank.Web.ViewModels.Transactions;
using Bank.Web.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace Bank.Web.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Customer, CustomerDetailsViewModel>().ReverseMap();
            CreateMap<Customer, CustomerSearchViewModel>().ReverseMap();
            CreateMap<Account, AccountCustomerViewModel>().ReverseMap();
            CreateMap<Transaction, TransactionDetailsViewModel>().ReverseMap();
            CreateMap<DepositViewModel, Transaction>();
            CreateMap<WithdrawViewModel, Transaction>();
            CreateMap<Transaction, TransactionConfirmationViewModel>();

            CreateMap<CustomerRegisterViewModel, Customer>()
                .ForMember(i => i.Gender, opt => opt.MapFrom(i => i.SelectedGender)); 

            CreateMap<CustomerEditViewModel, Customer>()
                .ForMember(i => i.Gender, opt => opt.MapFrom(i => i.SelectedGender));

            CreateMap<Customer, CustomerEditViewModel>()
                .ForMember(i => i.SelectedGender, opt => opt.MapFrom(i => i.Gender));

            CreateMap<IdentityUser, UserViewModel>();
            CreateMap<IdentityUser, UserEditViewModel>();

            CreateMap<Disposition, TopCustomerViewModel>()
                .ForMember(i => i.FirstName, opt => opt.MapFrom(c => c.Customer.Givenname))
                .ForMember(i => i.LastName, opt => opt.MapFrom(i => i.Customer.Surname))
                .ForMember(i => i.TotalBalance, opt => opt.MapFrom(i => i.Account.Balance));
        }
    }
}