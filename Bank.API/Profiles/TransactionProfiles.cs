using AutoMapper;
using Bank.API.ViewModels;
using Bank.API.ViewModels.Transactions;
using Bank.Data.Models;

namespace Bank.API.Profiles
{
    public class TransactionProfiles : Profile
    {
        public TransactionProfiles()
        {
            CreateMap<Transaction, TransactionDetailViewModel>();
        }
    }
}