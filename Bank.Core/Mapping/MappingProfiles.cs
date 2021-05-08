using AutoMapper;
using Bank.Core.ViewModels.Accounts;
using Bank.Core.ViewModels.Customers;
using Bank.Core.ViewModels.Statistics;
using Bank.Core.ViewModels.Transactions;
using Bank.Core.ViewModels.User;
using Bank.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Bank.Core.Mapping
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
                .ForMember(i => i.Gender, opt => opt.MapFrom(i => i.SelectedGender)); //Double check that I Work, I guess well find out if I Crash First.
            CreateMap<CustomerEditViewModel, Customer>(); //Double check that I Work, I guess well find out if I Crash First.

            CreateMap<IdentityUser, UserViewModel>();
            CreateMap<IdentityUser, UserEditViewModel>();

            CreateMap<Disposition, TopCustomerViewModel>()
                .ForMember(i => i.FirstName, opt => opt.MapFrom(c => c.Customer.Givenname))
                .ForMember(i => i.LastName, opt => opt.MapFrom(i => i.Customer.Surname))
                .ForMember(i => i.TotalBalance, opt => opt.MapFrom(i => i.Account.Balance));
        }
    }
}