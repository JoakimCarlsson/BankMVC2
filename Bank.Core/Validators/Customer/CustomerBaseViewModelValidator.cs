using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Core.Extensions;
using Bank.Core.ViewModels.Customers;
using FluentValidation;

namespace Bank.Core.Validators.Customer
{
    public class CustomerBaseViewModelValidator : AbstractValidator<CustomerBaseViewModel>
    {
        public CustomerBaseViewModelValidator()
        {
            RuleFor(i => i.GivenName)
                .RequiredAndMaxAndMinLength(100);

            RuleFor(i => i.Surname)
                .RequiredAndMaxAndMinLength(100);

            RuleFor(i => i.StreetAddress)
                .RequiredAndMaxAndMinLength(100);

            RuleFor(i => i.City)
                .RequiredAndMaxAndMinLength(100);

            RuleFor(i => i.Zipcode)
                .RequiredAndMaxAndMinLength(100);

            RuleFor(i => i.Country)
                .RequiredAndMaxAndMinLength(100);

            RuleFor(i => i.CountryCode)
                .RequiredAndMaxAndMinLength(2);

            RuleFor(i => i.SelectedGender)
                .RequiredAndMaxAndMinLength(6);

            RuleFor(i => i.TelephoneCountryCode)
                .RequiredAndMaxAndMinLength(10);

            RuleFor(i => i.TelephoneNumber)
                .RequiredAndMaxAndMinLength(25);

            RuleFor(e => e.EmailAddress)
                .RequiredAndMaxAndMinLength(100)
                .EmailAddress();

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
