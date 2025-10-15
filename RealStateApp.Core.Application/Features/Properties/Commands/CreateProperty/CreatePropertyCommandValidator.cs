using FluentValidation;

namespace RealStateApp.Core.Application.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
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

            RuleFor(x => x.Images)
                .Must(images => images != null && images.Count > 0)
                .WithMessage("Debe incluir al menos una imagen.");

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
