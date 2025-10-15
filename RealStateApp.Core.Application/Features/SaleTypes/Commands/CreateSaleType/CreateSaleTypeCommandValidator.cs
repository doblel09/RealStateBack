using FluentValidation;

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands.CreateSaleType
{
    public class CreateSaleTypeCommandValidator : AbstractValidator<CreateSaleTypeCommand>
    {
        public CreateSaleTypeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del tipo de venta es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del tipo de venta no puede tener más de 100 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción del tipo de venta es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no puede tener más de 500 caracteres.");
        }
    }
}
