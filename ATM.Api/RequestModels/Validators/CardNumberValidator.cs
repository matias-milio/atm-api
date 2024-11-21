using FluentValidation;

namespace ATM.Api.RequestModels.Validators
{
    public class CardNumberValidator : AbstractValidator<string>
    {
        public CardNumberValidator()
        {
            RuleFor(cardNumber => cardNumber)
                .NotEmpty()
                .WithMessage("Card number is required.")
                .Length(16)
                .WithMessage("Card number must be 16 digits long.")
                .Matches(@"^\d{16}$")
                .WithMessage("Card number must contain only digits.");
        }
    }
}
