using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bank.Core.Extensions;
using Bank.Core.Services.User;
using Bank.Core.ViewModels.User;
using Bank.Data.Data;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Validators.User
{
    //todo fix me
    class UserRegisterViewModelValidator : AbstractValidator<UserRegisterViewModel>
    {
        private readonly IUserService _userService;

        public UserRegisterViewModelValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .EmailAddress()
                .MustAsync(BeUniqe).WithMessage("{PropertyName} already exists.")
                .MaximumLength(256);

            RuleFor(e => e.ConfirmEmail)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .EmailAddress()
                .MaximumLength(256)
                .Equal(i => i.Email);

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Password()
                .NotNull();

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .Password()
                .NotNull()
                .Equal(i => i.Password);


            RuleFor(i => i.Roles)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }

        private Task<bool> BeUniqe(string email, CancellationToken token)
        {
            return _userService.CheckIfEmailExists(email);
        }
    }
}
