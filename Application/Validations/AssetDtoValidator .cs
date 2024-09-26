using Application.Models;
using FluentValidation;

namespace Application.Validations
{
    public class AssetDtoValidator : AbstractValidator<AssetDto>
    {
        public AssetDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Asset name is required.")
                .MaximumLength(100).WithMessage("Asset name must be less than 100 characters.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Asset type is required.")
                .Must(type => type == "Stock" || type == "Bond")
                .WithMessage("Asset type must be either 'Stock' or 'Bond'.");

            RuleFor(x => x.CurrentMarketValue)
                .GreaterThanOrEqualTo(0).WithMessage("Current market value must be greater than or equal to 0.");

            RuleFor(x => x.CostBasis)
                .GreaterThanOrEqualTo(0).WithMessage("Cost basis must be greater than or equal to 0.");

            RuleFor(x => x.QuantityHeld)
                .GreaterThan(0).WithMessage("Quantity held must be greater than 0.");

            RuleFor(x => x.PortfolioId)
               .GreaterThan(0).WithMessage("PortfolioId must be a valid positive number.")
               .When(x => x.PortfolioId != 0, ApplyConditionTo.CurrentValidator);
        }
    }
}
