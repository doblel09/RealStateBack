using FluentValidation;

namespace RealStateApp.Core.Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id de la propiedad debe ser mayor a 0.");

            RuleFor(x => x.UniqueCode)
                .NotEmpty().WithMessage("El código único es obligatorio.")
                .Length(1, 50).WithMessage("El código único debe tener entre 1 y 50 caracteres.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

            RuleFor(x => x.SizeInSquareMeters)
                .GreaterThan(0).WithMessage("El tamaño debe ser mayor a 0 metros cuadrados.");

            RuleFor(x => x.RoomCount)
                .GreaterThanOrEqualTo(0).WithMessage("La cantidad de habitaciones no puede ser negativa.");

            RuleFor(x => x.BathroomCount)
                .GreaterThanOrEqualTo(0).WithMessage("La cantidad de baños no puede ser negativa.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no puede tener más de 500 caracteres.");

            RuleFor(x => x.PropertyTypeId)
                .GreaterThan(0).WithMessage("Debe proporcionar un Id de tipo de propiedad válido.");

            RuleFor(x => x.SaleTypeId)
                .GreaterThan(0).WithMessage("Debe proporcionar un Id de tipo de venta válido.");

            RuleFor(x => x.AgentId)
                .NotEmpty().WithMessage("Debe proporcionar un Id de agente.")
                .Length(36).WithMessage("El Id del agente debe tener 36 caracteres (formato GUID).");

            RuleFor(x => x.Improvements)
                .Must(improvements => improvements == null || improvements.All(id => id > 0))
                .WithMessage("Si se proporcionan, todos los Ids de mejoras deben ser mayores a 0.");
        }
    }
}
