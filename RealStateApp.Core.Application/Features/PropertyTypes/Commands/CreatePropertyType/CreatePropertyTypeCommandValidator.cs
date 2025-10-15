using FluentValidation;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType
{
    public class CreatePropertyTypeCommandValidator : AbstractValidator<CreatePropertyTypeCommand>
    {
        public CreatePropertyTypeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del tipo de propiedad es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del tipo de propiedad no puede tener más de 100 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción del tipo de propiedad es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no puede tener más de 500 caracteres.");
        }
    }
}
