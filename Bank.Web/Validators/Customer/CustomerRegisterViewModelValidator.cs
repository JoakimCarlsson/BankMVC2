using Bank.Web.ViewModels.Customers;
using FluentValidation;

namespace Bank.Web.Validators.Customer
{
    public class CustomerRegisterViewModelValidator : AbstractValidator<CustomerRegisterViewModel>
    {
        public CustomerRegisterViewModelValidator()
        {
            Include(new CustomerBaseViewModelValidator());
        }
    }
}
