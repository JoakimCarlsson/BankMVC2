using Bank.Core.Extensions;
using Bank.Core.ViewModels.Customers;
using FluentValidation;
using System;

namespace Bank.Core.Validators.Customer
{
    class CustomerRegisterViewModelValidator : AbstractValidator<CustomerRegisterViewModel>
    {
        public CustomerRegisterViewModelValidator()
        {
            Include(new CustomerBaseViewModelValidator());

            RuleFor(i => i.Birthday)
                .NotEmpty().WithMessage("{PropertyName} is requierd.")
                .NotNull()
                .LessThan(DateTime.Now).WithMessage("Can't be born in the future."); //
                                                                                     //.GreaterThan(DateTime.MinValue);

            RuleFor(i => i.NationalId)
                .RequiredAndMaxAndMinLength(20);
        }
    }
}
