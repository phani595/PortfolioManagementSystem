using Application.Models;
using FluentValidation;

namespace Application.Validations
{
    public class TransactionDtoValidator : AbstractValidator<TransactionsDto>
    {
        public TransactionDtoValidator()
        {
            RuleFor(x => x.TransactionDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transaction date cannot be in the future.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Transaction type is required.")
                .Must(type => type == "Buy" || type == "Sell")
                .WithMessage("Transaction type must be one of the following: Buy, Sell.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.Fees)
                .GreaterThanOrEqualTo(0).WithMessage("Fees must be non-negative.");

            RuleFor(x => x.AssetId)
                .GreaterThan(0).WithMessage("AssetId must be greater than 0.")
                .When(x => x.AssetId != 0, ApplyConditionTo.CurrentValidator);
        }
    }
}
