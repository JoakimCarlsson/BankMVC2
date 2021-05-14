using AutoMapper;
using Bank.Data.Models;
using Bank.Web.ViewModels.Transactions;

namespace Bank.Web.Mapping
{
    public class TransactionProfiles : Profile
    {
        public TransactionProfiles()
        {
            CreateMap<Transaction, TransactionDetailsViewModel>().ReverseMap();
            CreateMap<DepositViewModel, Transaction>();
            CreateMap<WithdrawViewModel, Transaction>();
            CreateMap<Transaction, TransactionConfirmationViewModel>();
        }
    }
}