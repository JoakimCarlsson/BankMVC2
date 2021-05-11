using AutoMapper;
using Bank.Data.Repositories.Account;
using Bank.Data.Repositories.Transaction;
using Bank.Web.Enums;
using Bank.Web.ViewModels.Transactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Data.Models;

namespace Bank.Web.Services.Transactions
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
                Transactions = _mapper.Map<IEnumerable<TransactionDetailsViewModel>>(await _transactionRepository.ListAllByAccountIdAsync(accountId).Result
                    .OrderByDescending(i => i.Date)
                    .ThenByDescending(i => i.TransactionId)
                    .Skip(skip)
                    .Take(take).ToListAsync().ConfigureAwait(false)),
                AccountId = accountId
            };
            return model;
        }

        public async Task<TransactionListViewModel> GetTransactions(int accountId, int offset, int limit)
        {
            var model = new TransactionListViewModel
            {
                Transactions = _mapper.Map<IEnumerable<TransactionDetailsViewModel>>(await _transactionRepository.ListAllByAccountIdAsync(accountId).Result
                    .Skip(offset)
                    .Take(limit).ToListAsync().ConfigureAwait(false)),
            };

            return model;
        }

        public async Task<TransactionResultViewModel> SaveTransaction(TransactionBaseViewModel model)
        {
            return model switch
            {
                DepositViewModel viewModel => await SaveDepositAsync(viewModel).ConfigureAwait(false),
                TransferViewModel viewModel => await SaveTransferAsync(viewModel).ConfigureAwait(false),
                WithdrawViewModel viewModel => await SaveWithdrawAsync(viewModel).ConfigureAwait(false),
                _ => new TransactionResultViewModel { Result = TransactionResultCode.Error, TransactionId = 0 }
            };
        }

        public async Task<TransactionConfirmationViewModel> GetConfirmation(int transactionId)
        {
            var transaction = await _transactionRepository.GetByIdAsync(transactionId).ConfigureAwait(false);
            var model = _mapper.Map<TransactionConfirmationViewModel>(transaction);
            return model;
        }

        private async Task<TransactionResultViewModel> SaveDepositAsync(DepositViewModel model)
        {
            var newTransaction = _mapper.Map<Transaction>(model);
            var account = await _accountRepository.GetByIdAsync(newTransaction.AccountId).ConfigureAwait(false);

            newTransaction.Balance += account.Balance + newTransaction.Amount;
            newTransaction.Operation = "Deposit";
            newTransaction.Type = "Credit";
            newTransaction.Date = DateTime.Now;

            account.Balance += newTransaction.Amount;

            await _accountRepository.UpdateAsync(account).ConfigureAwait(false);
            var transaction = await _transactionRepository.AddAsync(newTransaction).ConfigureAwait(false);

            return new TransactionResultViewModel { Result = TransactionResultCode.Success, TransactionId = transaction.TransactionId };
        }

        private async Task<TransactionResultViewModel> SaveTransferAsync(TransferViewModel model)
        {
            var fromAccount = await _accountRepository.GetByIdAsync(model.AccountId).ConfigureAwait(false);
            var toAccount = await _accountRepository.GetByIdAsync(model.ToAccountId).ConfigureAwait(false);

            var fromTransaction = new Transaction
            {
                Amount = -model.Amount,
                Operation = "Transfer to another account.",
                Type = "Credit",
                Date = DateTime.Now,
                AccountId = model.AccountId,
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

            await _accountRepository.UpdateAsync(fromAccount).ConfigureAwait(false);
            await _accountRepository.UpdateAsync(toAccount).ConfigureAwait(false);
            var transaction = await _transactionRepository.AddAsync(fromTransaction).ConfigureAwait(false);
            await _transactionRepository.AddAsync(toTransaction).ConfigureAwait(false);

            return new TransactionResultViewModel { Result = TransactionResultCode.Success, TransactionId = transaction.TransactionId };
        }

        private async Task<TransactionResultViewModel> SaveWithdrawAsync(WithdrawViewModel model)
        {
            var transaction = new Transaction();
            var account = await _accountRepository.GetByIdAsync(model.AccountId).ConfigureAwait(false);

            var balance = account.Balance - model.Amount;
            transaction.Balance = balance;
            transaction.Operation = "Withdraw";
            transaction.Type = "Credit";
            transaction.Date = DateTime.Now;
            transaction.Amount = -model.Amount;
            transaction.AccountId = model.AccountId;

            account.Balance -= -transaction.Amount;

            await _accountRepository.UpdateAsync(account).ConfigureAwait(false);
            var savedTransaction = await _transactionRepository.AddAsync(transaction).ConfigureAwait(false);

            return new TransactionResultViewModel { Result = TransactionResultCode.Success, TransactionId = savedTransaction.TransactionId };
        }
    }
}