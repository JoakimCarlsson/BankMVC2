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
    public class TransferViewModelValidator : AbstractValidator<TransferViewModel>
    {
        private readonly IAccountRepository _accountRepository;

        public TransferViewModelValidator(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;

            RuleFor(i => i.FromAccountId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MustAsync(AccountIdExists).WithMessage("{PropertyName} does not exist.")
                .NotEqual(i => i.ToAccountId).WithMessage("Can't transfer money too the same account");
                

            RuleFor(i => i.ToAccountId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MustAsync(AccountIdExists).WithMessage("{PropertyName} does not exist.")
                .NotEqual(i => i.FromAccountId).WithMessage("Can't transfer money too the same account"); //Not needed I guess? But better safe then sorry.


            RuleFor(i => i.Amount)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .GreaterThan(0).WithMessage("{PropertyName} can't be less or equal to 0.")
                .MustAsync(HaveCoverage).WithMessage("The account does not have enough money to do this transaction");
        }

        //todo refactor me
        private async Task<bool> HaveCoverage(TransferViewModel model, decimal amount, CancellationToken token)
        {
            if (await AccountIdExists(model.FromAccountId, new CancellationToken(false)))
                return _accountRepository.GetByIdAsync(model.FromAccountId).GetAwaiter().GetResult().Balance >= amount;

            return false;
        }

        private async Task<bool> AccountIdExists(int id, CancellationToken token)
        {
            var account = await _accountRepository.GetByIdAsync(id).ConfigureAwait(false);
            return account != null;
        }

        //private async Task<bool> MustBeUnique
    }
}
