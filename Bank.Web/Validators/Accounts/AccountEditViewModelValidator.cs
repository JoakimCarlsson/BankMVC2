using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Data.Repositories.Customer;
using Bank.Web.Services.Account;
using Bank.Web.ViewModels.Accounts;
using FluentValidation;

namespace Bank.Web.Validators.Accounts
{
    public class AccountEditViewModelValidator : AbstractValidator<AccountEditViewModel>
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerRepository _customerRepository;

        public AccountEditViewModelValidator(IAccountService accountService, ICustomerRepository customerRepository)
        {
            _accountService = accountService;
            _customerRepository = customerRepository;
            
            RuleFor(i => i.NewCustomerId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                // .MustAsync(CustomerIsAlreadyApartOfAccount).WithMessage("Customer is already added.")
                .MustAsync(MustExist).WithMessage("Customer does not exist.")
                .MustAsync(MustNotBeOwner).WithMessage("You silly goose, you can't remove the owner.");
        }

        private async Task<bool> MustNotBeOwner(AccountEditViewModel model, int customerId, CancellationToken token)
        {
            var users = await _accountService.GetAccountUsers(model.AccountId);
            return users.First(i => i.CustomerId == customerId).Type != "OWNER";
        }

        private async Task<bool> MustExist(AccountEditViewModel model, int customerId, CancellationToken token)
        {
            return await _customerRepository.GetByIdAsync(customerId) is not null;
        }

        // private async Task<bool> CustomerIsAlreadyApartOfAccount(AccountEditViewModel model, int customerId, CancellationToken token)
        // {
        //     var users = await _accountService.GetAccountUsers(model.AccountId);
        //     return users.FirstOrDefault(i => i.CustomerId == customerId) is null;
        // }
    }
}