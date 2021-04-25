using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Core.Repository.TranasctionsRep;
using Bank.Core.ViewModels.Customers;
using Bank.Core.ViewModels.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Services.Transactions
{
    public interface ITransactionService
    {
        public Task<TransactionDetailsListViewModel> GetByAllIdAsync(int accountId);
        public Task<TransactionDetailsListViewModel> GetAmountByIdAsync(int accountId, int skip, int take);
    }

    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IMapper mapper, ITransactionRepository transactionRepository)
        {
            _mapper = mapper;
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionDetailsListViewModel> GetByAllIdAsync(int accountId)
        {
            var model = new TransactionDetailsListViewModel
            {
                Transactions = _mapper.Map<IEnumerable<TransactionDetailsViewModel>>(await _transactionRepository.ListAllByAccountIdAsync(accountId)),
                AccountId = accountId
            };

            return model;
        }

        public async Task<TransactionDetailsListViewModel> GetAmountByIdAsync(int accountId, int skip, int take)
        {
            var model = new TransactionDetailsListViewModel
            {
                Transactions = _mapper.Map<IEnumerable<TransactionDetailsViewModel>>(await _transactionRepository.ListAllByAccountIdAsync(accountId).Result.Skip(skip).Take(take).ToListAsync()),
                AccountId = accountId
            };

            return model;
        }
    }
}