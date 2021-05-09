using Bank.Core.Extensions;
using Bank.Core.ViewModels.Customers;
using FluentValidation;
using System;

namespace Bank.Core.Validators.Customer
{
    public class CustomerRegisterViewModelValidator : AbstractValidator<CustomerRegisterViewModel>
    {
        public CustomerRegisterViewModelValidator()
        {
            Include(new CustomerBaseViewModelValidator());
        }
    }
}
