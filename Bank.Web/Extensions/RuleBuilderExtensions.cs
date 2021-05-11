using FluentValidation;

namespace Bank.Web.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            
            var options = ruleBuilder
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(8).WithMessage("{PropertyName} is must have at least 8 characters.")
                .MaximumLength(16).WithMessage("{PropertyName} cant be longer then 16 characters.")
                .Matches("[A-Z]+[a-z]+[0-9]+[^a-zA-Z0-9]").WithMessage("{PropertyName} is not a valid .");
            //.Matches("[A-Z]+").WithMessage("{PropertyName} must have an uppercase letter.") //In theory this should work, but it simply does not work. 
            //.Matches("[a-z]+").WithMessage("{PropertyName} must have an lowercase letter.")
            //.Matches("[0-9]+").WithMessage("{PropertyName} must have a number.")
            //.Matches("[^a-zA-Z0-9]").WithMessage("{PropertyName} must have a special character.");

            return options;
        }

        public static IRuleBuilder<T, string> RequiredAndMaxAndMinLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength, int minLength = 1)
        {

            var options = ruleBuilder
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MinimumLength(minLength).WithMessage($"{{PropertyName}} cannot be less then {minLength} characters.")
                .MaximumLength(maxLength).WithMessage($"{{PropertyName}} cannot exceed more then {maxLength} characters.");

            return options;
        }
    }
}
