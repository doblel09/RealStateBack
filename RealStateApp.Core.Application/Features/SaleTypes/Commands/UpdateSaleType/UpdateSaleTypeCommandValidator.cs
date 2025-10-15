using FluentValidation;

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands.UpdateSaleType
{
    public class UpdateSaleTypeCommandValidator : AbstractValidator<UpdateSaleTypeCommand>
    {
        public UpdateSaleTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id de tipo de venta debe ser mayor que 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del tipo de venta es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del tipo de venta no puede tener más de 100 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción del tipo de venta es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no puede tener más de 500 caracteres.");
        }
    }
}
