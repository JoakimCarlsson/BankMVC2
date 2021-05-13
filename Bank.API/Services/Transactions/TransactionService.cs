using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bank.API.ViewModels.Transactions;
using Bank.Data.Models;
using Bank.Data.Repositories.Transaction;

namespace Bank.API.Services.Transactions
{
    class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        
        public async Task<TransactionDetailsListViewModel> GetTransactions(int id, int limit, int offset)
        {
            var allTransactions = await _transactionRepository.ListAllByAccountIdAsync(id);
            var transactions = limit == 0 ? allTransactions.Skip(offset) : allTransactions.Skip(offset).Take(limit);
            
            return new TransactionDetailsListViewModel
            {
                Transactions = _mapper.Map<IEnumerable<TransactionDetailViewModel>>(transactions),
            };
        }
    }
}