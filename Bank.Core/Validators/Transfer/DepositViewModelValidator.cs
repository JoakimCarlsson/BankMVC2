﻿using System.Threading;
using System.Threading.Tasks;
using Bank.Core.Model;
using Bank.Core.Repository.AccountRep;
using Bank.Core.Repository.Base;
using Bank.Core.ViewModels.Transactions;
using FluentValidation;

namespace Bank.Core.Validators.Transfer
{
    public class DepositViewModelValidator : AbstractValidator<DepositViewModel>
    {
        private readonly IAsyncRepository<Account> _accountRepository;

        public DepositViewModelValidator(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;

            RuleFor(i => i.AccountId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MustAsync(AccountIdExists).WithMessage("{PropertyName} does not exist.");

            RuleFor(a => a.Amount)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .GreaterThan(0).WithMessage("{PropertyName} can't be less or equal to 0.");

        }

        private async Task<bool> AccountIdExists(int id, CancellationToken token)
        {
            var account = await _accountRepository.GetByIdAsync(id).ConfigureAwait(false);
            return account != null;
        }
    }
}