using System.Threading;
using System.Threading.Tasks;
using Bank.Web.Extensions;
using Bank.Web.Services.User;
using Bank.Web.ViewModels.User;
using FluentValidation;

namespace Bank.Web.Validators.User
{
    //todo fix me
    public class UserRegisterViewModelValidator : AbstractValidator<UserRegisterViewModel>
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
                .NotNull()
                .Password();

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .Equal(i => i.Password);


            RuleFor(i => i.Roles)
                .NotEmpty().WithMessage("You must select atleast one role.");
        }

        private Task<bool> BeUniqe(string email, CancellationToken token)
        {
            return _userService.CheckIfEmailExistsAsync(email);
        }
    }
}
