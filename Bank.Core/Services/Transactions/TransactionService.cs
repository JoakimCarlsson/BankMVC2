using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Core.Data;
using Bank.Core.Model;
using Bank.Core.Repository.AccountRep;
using Bank.Core.Repository.TranasctionsRep;
using Bank.Core.ViewModels.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public TransactionService(IMapper mapper, ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _mapper = mapper;
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
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
                Transactions = _mapper.Map<IEnumerable<TransactionDetailsViewModel>>(await _transactionRepository.ListAllByAccountIdAsync(accountId).Result.OrderByDescending(i => i.Date).Skip(skip).Take(take).ToListAsync()),
                AccountId = accountId
            };

            return model;
        }

        public async Task SaveDeposit(DepositViewModel model)
        {
            var newTransaction = _mapper.Map<Transaction>(model);
            var account = await _accountRepository.GetByIdAsync(newTransaction.AccountId).ConfigureAwait(false);

            newTransaction.Balance += account.Balance + newTransaction.Amount;
            newTransaction.Operation = "Deposit";
            newTransaction.Type = "Credit";
            newTransaction.Date = DateTime.Now;

            account.Balance += newTransaction.Amount;

            await _accountRepository.UpdateAsync(account).ConfigureAwait(false);
            await _transactionRepository.AddAsync(newTransaction).ConfigureAwait(false);
        }

        public async Task SaveTransferAsync(TransferViewModel model)
        {
            var fromAccount = await _accountRepository.GetByIdAsync(model.FromAccountId).ConfigureAwait(false);
            var toAccount = await _accountRepository.GetByIdAsync(model.ToAccountId).ConfigureAwait(false);

            var fromTransaction = new Transaction
            {
                Amount = -model.Amount,
                Operation = "Transfer to another account.",
                Type = "Credit",
                Date = DateTime.Now,
                AccountId = model.FromAccountId,
                Balance = fromAccount.Balance -= model.Amount,
            };

            var toTransaction = new Transaction
            {
                Amount = model.Amount,
                Operation = "Transfer from another account.",
                Type = "Credit",
                Date = DateTime.Now,
                AccountId = model.ToAccountId,
                Balance = toAccount.Balance += model.Amount,
            };

            await _accountRepository.UpdateAsync(fromAccount).ConfigureAwait(true);
            await _accountRepository.UpdateAsync(toAccount).ConfigureAwait(true);
            await _transactionRepository.AddAsync(fromTransaction).ConfigureAwait(true);
            await _transactionRepository.AddAsync(toTransaction).ConfigureAwait(true);
        }
    }
}