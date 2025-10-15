using FluentValidation;
using RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties;

namespace RealStateApp.Core.Application.Features.Properties.Validators
{
    public class GetAllPropertiesQueryValidator : AbstractValidator<GetAllPropertiesQuery>
    {
        public GetAllPropertiesQueryValidator()
        {
            // Validación para PropertyTypeId: si se proporciona, debe ser mayor a 0
            RuleFor(x => x.PropertyTypeId)
                .GreaterThan(0)
                .When(x => x.PropertyTypeId.HasValue)
                .WithMessage("PropertyTypeId must be greater than 0.");

            // Validación para SaleTypeId: si se proporciona, debe ser mayor a 0
            RuleFor(x => x.SaleTypeId)
                .GreaterThan(0)
                .When(x => x.SaleTypeId.HasValue)
                .WithMessage("SaleTypeId must be greater than 0.");

            
            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinPrice.HasValue)
                .WithMessage("MinPrice must be greater than or equal to 0.");

            
            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MaxPrice.HasValue)
                .WithMessage("MaxPrice must be greater than or equal to 0.");

            
            RuleFor(x => x.UniqueCode)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.UniqueCode))
                .WithMessage("UniqueCode cannot be empty.");
        }
    }
}
