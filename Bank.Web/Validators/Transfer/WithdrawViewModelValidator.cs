using System.Threading;
using System.Threading.Tasks;
using Bank.Data.Repositories.Account;
using Bank.Web.ViewModels.Transactions;
using FluentValidation;

namespace Bank.Web.Validators.Transfer
{
    public class WithdrawViewModelValidator : AbstractValidator<WithdrawViewModel>
    {
        private readonly IAccountRepository _accountRepository;

        public WithdrawViewModelValidator(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;

            RuleFor(i => i.AccountId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MustAsync(AccountIdExists).WithMessage("{PropertyName} does not exist.");


            RuleFor(a => a.Amount)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .GreaterThan(0).WithMessage("{PropertyName} can't be less or equal to 0.")
                .MustAsync(HaveCoverage).WithMessage("The account does not have enough money to do this transaction")
                .ScalePrecision(2, int.MaxValue);
        }

        //TODO, refactor me.
        private async Task<bool> AccountIdExists(int id, CancellationToken token)
        {
            var account = await _accountRepository.GetByIdAsync(id).ConfigureAwait(false);
            return account != null;
        }

        private async Task<bool> HaveCoverage(WithdrawViewModel model, decimal amount, CancellationToken token)
        {
            if (await AccountIdExists(model.AccountId, new CancellationToken(false)))
                return _accountRepository.GetByIdAsync(model.AccountId).GetAwaiter().GetResult().Balance >= amount;

            return false;
        }
    }
}
