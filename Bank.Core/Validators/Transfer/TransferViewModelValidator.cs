using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bank.Core.Repository.AccountRep;
using Bank.Core.ViewModels.Transactions;
using FluentValidation;

namespace Bank.Core.Validators.Transfer
{
    class TransferViewModelValidator : AbstractValidator<TransferViewModel>
    {
        private readonly IAccountRepository _accountRepository;

        public TransferViewModelValidator(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;

            RuleFor(i => i.FromAccountId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MustAsync(AccountIdExists).WithMessage("{PropertyName} does not exist.")
                .NotEqual(i => i.ToAccountId).WithMessage("Can't transfer money too the same account")
                .MustAsync(HaveCoverage).WithMessage("{PropertyName} does not have enough money to do this transaction");

            RuleFor(i => i.ToAccountId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MustAsync(AccountIdExists).WithMessage("{PropertyName} does not exist.")
                .NotEqual(i => i.FromAccountId).WithMessage("Can't transfer money too the same account"); //Not needed I guess? But better safe then sorry.


            RuleFor(i => i.Amount)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .GreaterThan(0).WithMessage("{PropertyName} can't be less or equal to 0."); ;
        }

        private async Task<bool> HaveCoverage(TransferViewModel model, int id, CancellationToken token)
        {
            return _accountRepository.GetByIdAsync(id).GetAwaiter().GetResult().Balance >= model.Amount;
        }

        private async Task<bool> AccountIdExists(int id, CancellationToken token)
        {
            var account = await _accountRepository.GetByIdAsync(id).ConfigureAwait(false);
            return account != null;
        }

        //private async Task<bool> MustBeUnique
    }
}
