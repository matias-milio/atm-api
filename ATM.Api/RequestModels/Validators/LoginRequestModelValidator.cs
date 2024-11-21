using FluentValidation;

namespace ATM.Api.RequestModels.Validators
{
    public class LoginRequestModelValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestModelValidator()
        {
            RuleFor(model => model.PIN)
             .NotEmpty()
                .WithMessage("PIN is required.")
                .Length(4)
                .WithMessage("PIN must be 4 digits long.")
                .Matches(@"^\d{16}$")
                .WithMessage("PIN number must contain only digits.");

            RuleFor(model => model.CardNumber)
                .NotEmpty()
                .WithMessage("Card number is required.")
                .Length(16)
                .WithMessage("Card number must be 16 digits long.")
                .Matches(@"^\d{16}$")
                .WithMessage("Card number must contain only digits.");
        }
    }
}
