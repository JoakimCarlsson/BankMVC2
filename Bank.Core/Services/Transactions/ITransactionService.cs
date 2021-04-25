using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Core.Repository.TranasctionsRep;
using Bank.Core.ViewModels.Customers;
using Bank.Core.ViewModels.Transactions;

namespace Bank.Core.Services.Transactions
{
    public interface ITransactionService
    {
        public Task<TransactionDetailsListViewModel> GetByIdAsync(int accountId);
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

        public async Task<TransactionDetailsListViewModel> GetByIdAsync(int accountId)
        {
            var model = new TransactionDetailsListViewModel
            {
                Transactions = _mapper.Map<IEnumerable<TransactionDetailsViewModel>>(await _transactionRepository.ListAllByAccountIdAsync(accountId))
            };

            return model;
        }
    }
}