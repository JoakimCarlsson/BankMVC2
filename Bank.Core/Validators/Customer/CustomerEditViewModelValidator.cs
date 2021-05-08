using Bank.Core.Extensions;
using Bank.Core.ViewModels.Customers;
using FluentValidation;
using System;

namespace Bank.Core.Validators.Customer
{
    class CustomerEditViewModelValidator: AbstractValidator<CustomerEditViewModel>
    {
        public CustomerEditViewModelValidator()
        {
            Include(new CustomerBaseViewModelValidator());
        }
    }
}
