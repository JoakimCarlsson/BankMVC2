using Bank.Web.ViewModels.Customers;
using FluentValidation;

namespace Bank.Web.Validators.Customer
{
    public class CustomerEditViewModelValidator: AbstractValidator<CustomerEditViewModel>
    {
        public CustomerEditViewModelValidator()
        {
            Include(new CustomerBaseViewModelValidator());
        }
    }
}
