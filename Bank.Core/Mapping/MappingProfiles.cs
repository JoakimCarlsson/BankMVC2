using AutoMapper;
using Bank.Core.Model;
using Bank.Core.ViewModels.Accounts;
using Bank.Core.ViewModels.Customers;
using Bank.Core.ViewModels.Transactions;

namespace Bank.Core.Mapping
{
    class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Customer, CustomerDetailsViewModel>().ReverseMap();
            CreateMap<Customer, CustomerSearchViewModel>().ReverseMap();
            CreateMap<Account, AccountCustomerViewModel>().ReverseMap();
            CreateMap<Transaction, TransactionDetailsViewModel>().ReverseMap();
            CreateMap<DepositViewModel, Transaction>();
            CreateMap<WithdrawViewModel, Transaction>();
        }
    }
}
