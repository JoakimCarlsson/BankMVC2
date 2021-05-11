using System.Threading;
using System.Threading.Tasks;
using Bank.Web.Services.User;
using Bank.Web.ViewModels.User;
using FluentValidation;

namespace Bank.Web.Validators.User
{
    //todo fix me
    public class UserEditViewModelValidator : AbstractValidator<UserEditViewModel>
    {
        private readonly IUserService _userService;

        public UserEditViewModelValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .EmailAddress()
                .MustAsync(BeUniqe).WithMessage("{PropertyName} already exists.")
                .MaximumLength(256);
        }

        private async Task<bool> BeUniqe(UserEditViewModel model, string email, CancellationToken token)
        {
            var user = await _userService.GetUserByIdAsync(model.Id).ConfigureAwait(false);
            if (user.Email == email) //email did not change,
                return true;

            return await _userService.CheckIfEmailExistsAsync(email).ConfigureAwait(false);
        }
    }
}
