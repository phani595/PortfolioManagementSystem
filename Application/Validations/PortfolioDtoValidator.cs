using Application.Models;
using FluentValidation;

namespace Application.Validations
{
    public class PortfolioDtoValidator : AbstractValidator<PortfolioDto>
    {
        public PortfolioDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Portfolio name is required.")
                .Length(3, 100).WithMessage("Portfolio name must be between 3 and 100 characters.");

            RuleForEach(x => x.Assets)
                .SetValidator(new AssetDtoValidator());
        }
    }
}
