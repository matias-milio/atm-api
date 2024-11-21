using FluentValidation;

namespace ATM.Api.RequestModels.Validators{
    public class WithdrawRequestModelValidator : AbstractValidator<WithdrawRequest>
    {
        public WithdrawRequestModelValidator()
        {
            RuleFor(model=> model.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.");
        }
    }
}
