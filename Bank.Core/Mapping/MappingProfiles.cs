using AutoMapper;
using Bank.Core.ViewModels.Accounts;
using Bank.Core.ViewModels.Customers;
using Bank.Core.ViewModels.Statistics;
using Bank.Core.ViewModels.Transactions;
using Bank.Data.Models;

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

            CreateMap<Disposition, TopCustomerViewModel>()
                .ForMember(i => i.FirstName, opt => opt.MapFrom(c => c.Customer.Givenname))
                .ForMember(i => i.LastName, opt => opt.MapFrom(i => i.Customer.Surname))
                .ForMember(i => i.TotalBalance, opt => opt.MapFrom(i => i.Account.Balance));
        }
    }
}